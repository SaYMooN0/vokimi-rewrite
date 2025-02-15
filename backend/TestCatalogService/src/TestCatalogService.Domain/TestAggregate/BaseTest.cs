using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.tests;
using System.Collections.Immutable;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.TestAggregate.formats_shared;

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
    protected TestInteractionsAccessSettings InteractionsAccessSettings { get; init; }
    private HashSet<TestCommentId> _commentIds { get; init; }
    private HashSet<TestRatingId> _ratingIds { get; init; }

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
        _ratingIds = [];
    }

    public ImmutableHashSet<TestCommentId> CommentIds => _commentIds.ToImmutableHashSet();

    public ImmutableHashSet<TestRatingId> RatingIds => _ratingIds.ToImmutableHashSet();

    private bool IsUserCreatorOrEditor(AppUserId userId) =>
        userId == CreatorId || EditorIds.Contains(userId);

    private async Task<bool> CheckUserFollowsCreator(
        Func<AppUserId, Task<ImmutableArray<AppUserId>>> getUserFollowings,
        AppUserId userId
    ) => (await getUserFollowings(userId)).Contains(userId);

    //comments 
    //ratings
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
        Func<AppUserId,
            Task<ImmutableArray<AppUserId>>> getUserFollowings
    ) => await InteractionsAccessSettings.CheckUserAccessToRate(
        userId, IsUserCreatorOrEditor,
        async (uId) => await CheckUserFollowsCreator(getUserFollowings, uId)
    );

    public async Task<ErrOrNothing> CheckUserAccessToComment(
        AppUserId userId,
        Func<AppUserId,
            Task<ImmutableArray<AppUserId>>> getUserFollowings
    ) => await InteractionsAccessSettings.CheckUserAccessToComment(
        userId, IsUserCreatorOrEditor,
        async (uId) => await CheckUserFollowsCreator(getUserFollowings, uId)
    );
}