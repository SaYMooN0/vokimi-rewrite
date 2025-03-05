using System.Collections.Immutable;
using SharedKernel.Common;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
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
    public TestInteractionsAccessSettings InteractionsAccessSettings { get; private set; }
    public ImmutableArray<TestFeedbackRecordId> _feedbackRecords { get; }
    protected ICollection<TestCommentReport> _commentReports { get; }
    private ImmutableHashSet<TestTagId> _tags { get; set; }
    private ICollection<TagSuggestionForTest> _tagSuggestions { get; }
    private ImmutableHashSet<TestTagId> TagsBannedFromSuggestion { get; set; }


    protected BaseTest(
        TestId testId,
        AppUserId creatorId,
        ImmutableArray<AppUserId> editorIds,
        DateTime publicationDate
    ) {
        Id = testId;
        CreatorId = creatorId;
        EditorIds = editorIds;
        PublicationDate = publicationDate;
        _feedbackRecords = [];
        _commentReports = [];
        _tagSuggestions = [];
        TagsBannedFromSuggestion = [];
    }

    public ErrOrNothing CheckUserAccessToManageTest(AppUserId userId) {
        if (userId != CreatorId) {
            return Err.ErrFactory.NoAccess("Only test creator has access to manage test");
        }

        return ErrOrNothing.Nothing;
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

    public ErrOrNothing AddTagSuggestions(HashSet<TestTagId> suggestedTags) {
        if (!InteractionsAccessSettings.AllowTagsSuggestions) {
            return new Err(
                "Tag suggestions are not allowed for this test",
                details: "Test creator disabled tag suggestions for this test");
        }

        var alreadyAddedTags = suggestedTags.Intersect(_tags).ToArray();
        var bannedTags = suggestedTags.Intersect(TagsBannedFromSuggestion).ToArray();
        if (
            alreadyAddedTags.Length + bannedTags.Length >= suggestedTags.Count()
        ) {
            return new Err(
                "Cannot accept tag suggestions because every tag is either already in test or banned from suggestion"
            );
        }

        var tagsToAddSuggestions = suggestedTags
            .Except(alreadyAddedTags)
            .Except(bannedTags)
            .ToArray();
    }

    
}