using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.tests;
using System.Collections.Immutable;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestCatalogService.Application.Common.interfaces.repositories;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.TestAggregate.formats_shared;
using TestCatalogService.Domain.TestAggregate.formats_shared.events;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Domain.TestAggregate;

public abstract class BaseTest : AggregateRoot<TestId>
{
    protected BaseTest() { }
    public abstract TestFormat Format { get; }
    public string Name { get; init; }
    public string CoverImg { get; init; }
    public string Description { get; init; }
    public AppUserId CreatorId { get; init; }
    public ImmutableArray<AppUserId> EditorIds { get; init; }
    public DateTime PublicationDate { get; init; }
    public Language Language { get; init; }
    public ImmutableHashSet<TestTagId> Tags { get; protected set; }
    public TestInteractionsAccessSettings InteractionsAccessSettings { get; protected init; }
    private HashSet<TestCommentId> _commentIds { get; init; }
    private ICollection<TestRating> _ratings { get; init; }

    protected BaseTest(
        TestId testId,
        string name, string coverImg, string description, AppUserId creatorId,
        ImmutableArray<AppUserId> editorIds, DateTime publicationDate, Language language,
        ImmutableHashSet<TestTagId> tags,
        TestInteractionsAccessSettings interactionsAccessSettings
    ) {
        Id = testId;
        Name = name;
        CoverImg = coverImg;
        Description = description;
        CreatorId = creatorId;
        EditorIds = editorIds;
        PublicationDate = publicationDate;
        Language = language;
        Tags = tags;
        InteractionsAccessSettings = interactionsAccessSettings;
        _commentIds = [];
        _ratings = [];
    }

    public ImmutableHashSet<TestCommentId> CommentIds => _commentIds.ToImmutableHashSet();

    public ImmutableArray<TestRating> Ratings => _ratings.ToImmutableArray();

    private bool IsUserCreatorOrEditor(AppUserId userId) =>
        userId == CreatorId || EditorIds.Contains(userId);

    private async Task<bool> CheckUserFollowsCreator(
        Func<AppUserId, Task<ImmutableArray<AppUserId>>> getUserFollowings,
        AppUserId userId
    ) => (await getUserFollowings(userId)).Contains(userId);

    public ErrOrNothing CheckAccessToViewTestForUnauthorized() => InteractionsAccessSettings.TestAccess switch {
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

    public async Task<ErrOrNothing> CheckUserAccessToViewTest(
        AppUserId userId,
        Func<AppUserId, Task<ImmutableArray<AppUserId>>> getUserFollowings
    ) => await InteractionsAccessSettings.CheckUserAccessToViewTest(
        userId, IsUserCreatorOrEditor,
        async (uId) => await CheckUserFollowsCreator(getUserFollowings, uId)
    );

    public async Task<ErrOrNothing> CheckUserAccessToRate(
        AppUserId userId,
        Func<AppUserId, Task<ImmutableArray<AppUserId>>> getUserFollowings
    ) => await InteractionsAccessSettings.CheckUserAccessToRate(
        userId, IsUserCreatorOrEditor,
        async (uId) => await CheckUserFollowsCreator(getUserFollowings, uId)
    );

    public async Task<ErrOrNothing> CheckUserAccessToComment(
        AppUserId userId,
        Func<AppUserId, Task<ImmutableArray<AppUserId>>> getUserFollowings
    ) => await InteractionsAccessSettings.CheckUserAccessToComment(
        userId, IsUserCreatorOrEditor,
        async (uId) => await CheckUserFollowsCreator(getUserFollowings, uId)
    );

    //CheckUserAccessToComment must be checked before
    public async Task<ErrOr<TestComment>> AddComment(
        AppUserId authorId,
        string commentText,
        TestCommentAttachment? attachment,
        bool markedAsSpoiler,
        ITestCommentsRepository testCommentsRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        if (attachment is not null && !attachment.RelatedEnumType.IsPossibleForTestFormat(Format)) {
            return new Err(
                $"Test with format {Format} does not support comment attachments with type {attachment.RelatedEnumType}"
            );
        }

        var creationRes = TestComment.CreateNew(
            Id, authorId, commentText, attachment, markedAsSpoiler, dateTimeProvider
        );
        if (creationRes.IsErr(out var err)) {
            return err;
        }

        var comment = creationRes.GetSuccess();
        await testCommentsRepository.Add(comment);
        _commentIds.Add(comment.Id);
        _domainEvents.Add(new TestCommentToTestAddedEvent(comment.Id, comment.AuthorId));
        return comment;
    }

    //CheckUserAccessToComment must be checked before
    public async Task<ErrOr<TestComment>> AddAnswerToComment(
        TestCommentId parentCommentId,
        AppUserId authorId,
        string commentText,
        TestCommentAttachment? attachment,
        bool markedAsSpoiler,
        ITestCommentsRepository testCommentsRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        TestComment? parentComment = await testCommentsRepository.GetById(parentCommentId);
        if (parentComment is null) {
            return Err.ErrFactory.NotFound("Cannot find parent comment");
        }

        if (parentComment.TestId != Id) {
            return new Err("Parent comment doesn't belong to this test");
        }

        var answerCreationRes = await AddComment(
            authorId, commentText, attachment, markedAsSpoiler, testCommentsRepository, dateTimeProvider
        );
        if (answerCreationRes.IsErr(out var err)) {
            return err;
        }

        parentComment.AddAnswer(answerCreationRes.GetSuccess());
        await testCommentsRepository.Add(parentComment);
        return answerCreationRes.GetSuccess();
    }

    // public ErrOr<TestRating> AddRating(AppUserId userId, ushort ratingValue) { }
    // public ErrOrNothing UpdateRating(AppUserId userId,Func<>, ushort ratingValue) { }
}