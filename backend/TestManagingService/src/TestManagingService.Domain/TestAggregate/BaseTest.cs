using System.Collections.Immutable;
using SharedKernel.Common;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using SharedKernel.Common.tests;
using SharedKernel.Common.tests.value_objects;
using TestManagingService.Domain.Common;
using TestManagingService.Domain.TestAggregate.formats_shared;
using TestManagingService.Domain.TestAggregate.formats_shared.comment_reports;
using TestManagingService.Domain.TestAggregate.formats_shared.events;

namespace TestManagingService.Domain.TestAggregate;

public abstract class BaseTest : AggregateRoot<TestId>
{
    protected BaseTest() { }
    public abstract TestFormat Format { get; }
    public AppUserId CreatorId { get; }
    public ImmutableArray<AppUserId> EditorIds { get; }
    public DateTime PublicationDate { get; }
    private TestInteractionsAccessSettings _interactionsAccessSettings { get; set; }
    public ImmutableArray<TestFeedbackRecordId> _feedbackRecords { get; init; }
    protected ICollection<TestCommentReport> _commentReports { get; init; }
    private ImmutableHashSet<TestTagId> _tags { get; set; }
    private ICollection<TagSuggestionForTest> _tagSuggestions { get; set; }
    private ImmutableHashSet<TestTagId> TagsBannedFromSuggestion { get; set; }

    protected BaseTest(
        TestId testId,
        AppUserId creatorId,
        ImmutableArray<AppUserId> editorIds,
        DateTime publicationDate,
        TestInteractionsAccessSettings interactionsAccessSettings
    ) {
        Id = testId;
        CreatorId = creatorId;
        EditorIds = editorIds;
        PublicationDate = publicationDate;
        _interactionsAccessSettings = interactionsAccessSettings;
        _feedbackRecords = [];
        _commentReports = [];
        _tags = [];
        _tagSuggestions = [];
        TagsBannedFromSuggestion = [];
    }

    public ImmutableArray<TagSuggestionForTest> TagSuggestions => _tagSuggestions.ToImmutableArray();
    public ImmutableArray<TestTagId> Tags => _tags.ToImmutableArray();

    public ErrOrNothing CheckUserAccessToManageTest(AppUserId userId) {
        if (userId != CreatorId) {
            return Err.ErrFactory.NoAccess("Only test creator has access to manage test");
        }

        return ErrOrNothing.Nothing;
    }

    private bool IsUserCreatorOrEditor(AppUserId userId) =>
        userId == CreatorId || EditorIds.Contains(userId);

    public async Task<ErrOrNothing> CheckUserAccessToViewTest(
        AppUserId userId,
        Func<AppUserId, Task<ImmutableArray<AppUserId>>> getUserFollowings
    ) => await _interactionsAccessSettings.CheckUserAccessToViewTest(
        userId, IsUserCreatorOrEditor,
        async (uId) => (await getUserFollowings(userId)).Contains(userId)
    );

    public ErrOrNothing CheckAccessToViewTestForUnauthorized() => _interactionsAccessSettings.TestAccess switch {
        AccessLevel.Public => ErrOrNothing.Nothing,
        AccessLevel.Private => Err.ErrFactory.NoAccess(
            "To access this test you must be either test creator or editor. Log in to your account if you are"
        ),
        AccessLevel.FollowersOnly =>
            Err.ErrFactory.NoAccess(
                "To access this test you must follow the test creator. Log in to your account, firstly"
            ),
        _ => throw new ArgumentException(
            $"Incorrect access level in the {nameof(CheckAccessToViewTestForUnauthorized)}"),
    };

    public ErrListOr<TestInteractionsAccessSettings> UpdateInteractionsAccessSettings(
        AccessLevel testAccessLevel,
        ResourceAvailabilitySetting ratingsSetting,
        ResourceAvailabilitySetting commentsSetting,
        bool allowTestTakenPosts,
        bool allowTagSuggestions
    ) {
        var updateResult = _interactionsAccessSettings.Update(
            testAccessLevel,
            ratingsSetting: ratingsSetting,
            commentsSetting: commentsSetting,
            allowTestTakenPosts,
            allowTagSuggestions
        );
        if (updateResult.IsErr(out var errList)) {
            return errList;
        }

        TestInteractionsAccessSettingsUpdatedEvent domainEvent = new(
            Id, _interactionsAccessSettings.TestAccess,
            AllowRatings: _interactionsAccessSettings.AllowRatings,
            AllowComments: _interactionsAccessSettings.AllowComments,
            AllowTestTakenPosts: _interactionsAccessSettings.AllowTestTakenPosts,
            AllowTagSuggestions: _interactionsAccessSettings.AllowTagsSuggestions
        );
        _domainEvents.Add(domainEvent);
        return _interactionsAccessSettings;
    }

    public ErrOrNothing UpdateTags(HashSet<TestTagId> newTags) {
        if (newTags.Count() > TestTagsRules.MaxTagsForTestCount) {
            return Err.ErrFactory.InvalidData(
                $"Too many tags selected. Test cannot have more than {TestTagsRules.MaxTagsForTestCount}",
                details: $"{newTags.Count()} tags selected. Maximum tags allowed: {TestTagsRules.MaxTagsForTestCount}"
            );
        }

        if (_tags.SetEquals(newTags)) {
            return new Err("Tag list has not been changed");
        }

        _tags = newTags.ToImmutableHashSet();
        _domainEvents.Add(new TagsChangedEvent(Id, newTags));
        return ErrOrNothing.Nothing;
    }

    public const int MaxSuggestionsCountToInteract = 200;

    public ErrOrNothing AddTagSuggestions(HashSet<TestTagId> suggestedTags, IDateTimeProvider dateTimeProvider) {
        if (!_interactionsAccessSettings.AllowTagsSuggestions) {
            return new Err(
                "Tag suggestions are not allowed for this test",
                details: "Test creator disabled tag suggestions for this test"
            );
        }

        if (suggestedTags.Count > MaxSuggestionsCountToInteract) {
            return new Err(
                $"Cannot add more than {MaxSuggestionsCountToInteract} tag suggestions at once. Please split your list of suggestions into multiple requests."
            );
        }

        var alreadyAddedTags = suggestedTags.Intersect(_tags).ToArray();
        var bannedTags = suggestedTags.Intersect(TagsBannedFromSuggestion).ToArray();
        if (alreadyAddedTags.Length + bannedTags.Length >= suggestedTags.Count) {
            return new Err(
                "Cannot accept tag suggestions because every tag is either already in the test or banned from suggestion."
            );
        }

        TestTagId[] correctTagSuggestions = suggestedTags
            .Except(alreadyAddedTags)
            .Except(bannedTags)
            .ToArray();
        HashSet<TestTagId> existingSuggestions = _tagSuggestions
            .Select(t => t.Tag)
            .ToHashSet();
        HashSet<TestTagId> suggestionsToIncrement = correctTagSuggestions
            .Where(existingSuggestions.Contains)
            .ToHashSet();

        foreach (var suggestion in _tagSuggestions) {
            if (suggestionsToIncrement.Contains(suggestion.Tag)) {
                suggestion.IncrementCount();
            }
        }

        var suggestionsToAdd = correctTagSuggestions
            .Except(suggestionsToIncrement)
            .Select(t => TagSuggestionForTest.CreateNew(t, dateTimeProvider));

        _tagSuggestions = _tagSuggestions.Union(suggestionsToAdd).ToList();
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing AcceptTagSuggestions(HashSet<TestTagId> tagToAccept) {
        if (tagToAccept.Count > MaxSuggestionsCountToInteract) {
            return new Err($"Cannot accept more than {MaxSuggestionsCountToInteract} suggestions at once");
        }

        if (tagToAccept.Count == 0) {
            return new Err("No tags specified for acceptance");
        }

        TagSuggestionForTest[] suggestionsToAccept = _tagSuggestions
            .Where(s => tagToAccept.Contains(s.Tag))
            .ToArray();

        if (suggestionsToAccept.Length < tagToAccept.Count) {
            HashSet<TestTagId> notSuggestedTags = [..tagToAccept];
            notSuggestedTags.ExceptWith(suggestionsToAccept.Select(s => s.Tag));

            return Err.ErrFactory.InvalidData(
                "Unable to accept tag suggestions because one or more tags were not suggested. Please remove these tags now and add them manually.",
                details: $"Tags that are not in the suggestions list: {string.Join(", ", notSuggestedTags)}"
            );
        }

        string[] alreadyAcceptedTagValues = tagToAccept
            .Where(t => _tags.Contains(t))
            .Select(t => t.Value)
            .ToArray();
        if (alreadyAcceptedTagValues.Length > 0) {
            return Err.ErrFactory.InvalidData(
                "Some tags have already been accepted.",
                details: $"Already accepted tags: {string.Join(", ", alreadyAcceptedTagValues)}"
            );
        }

        HashSet<TestTagId> newAllTestTags = _tags.Union(tagToAccept).ToHashSet();
        var updateRes = UpdateTags(newAllTestTags);
        if (updateRes.IsErr(out var err)) {
            return err;
        }

        foreach (var suggestion in suggestionsToAccept) {
            _tagSuggestions.Remove(suggestion);
        }

        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing DeclineTagSuggestions(HashSet<TestTagId> tagsToDecline) {
        if (tagsToDecline.Count > MaxSuggestionsCountToInteract) {
            return new Err($"Cannot decline more than {MaxSuggestionsCountToInteract} suggestions at once");
        }

        var suggestionsToRemove = _tagSuggestions.Where(s => tagsToDecline.Contains(s.Tag)).ToArray();
        if (suggestionsToRemove.Length < tagsToDecline.Count) {
            HashSet<TestTagId> notFoundTags = [..tagsToDecline];
            notFoundTags.ExceptWith(suggestionsToRemove.Select(s => s.Tag));

            return Err.ErrFactory.InvalidData(
                "Unable to decline some tag suggestions because they were not found.",
                details: $"Tags not found: {string.Join(", ", notFoundTags)}"
            );
        }

        foreach (var suggestion in suggestionsToRemove) {
            _tagSuggestions.Remove(suggestion);
        }

        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing DeclineAndBanTagSuggestions(HashSet<TestTagId> tagsToDeclineAndBan) {
        if (tagsToDeclineAndBan.Count > MaxSuggestionsCountToInteract) {
            return new Err($"Cannot decline more than {MaxSuggestionsCountToInteract} suggestions at once");
        }

        var suggestionsToRemove = _tagSuggestions.Where(s => tagsToDeclineAndBan.Contains(s.Tag)).ToArray();
        if (suggestionsToRemove.Length < tagsToDeclineAndBan.Count) {
            HashSet<TestTagId> notFoundTags = new HashSet<TestTagId>(tagsToDeclineAndBan);
            notFoundTags.ExceptWith(suggestionsToRemove.Select(s => s.Tag));

            return Err.ErrFactory.InvalidData(
                "Unable to decline some tag suggestions because they were not found as suggestions",
                details: $"Tags not found: {string.Join(", ", notFoundTags)}"
            );
        }

        foreach (var suggestion in suggestionsToRemove) {
            _tagSuggestions.Remove(suggestion);
            TagsBannedFromSuggestion = TagsBannedFromSuggestion.Add(suggestion.Tag);
        }

        return ErrOrNothing.Nothing;
    }

    public void AddCommentReport(
        AppUserId authorId,
        TestCommentId commentId,
        string text,
        CommentReportReason reason,
        IDateTimeProvider dateTimeProvider
    ) {
        TestCommentReport newReport = TestCommentReport.CreateNew(authorId, commentId, text, reason, dateTimeProvider);
        _commentReports.Add(newReport);
    }
}