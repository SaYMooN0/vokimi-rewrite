using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.tests;
using System.Collections.Immutable;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using SharedUserRelationsContext.repository;
using TestCatalogService.Domain.AppUserAggregate;
using TestCatalogService.Domain.AppUserAggregate.events;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.Common.filters;
using TestCatalogService.Domain.Common.interfaces.repositories;
using TestCatalogService.Domain.TestAggregate.formats_shared;
using TestCatalogService.Domain.TestAggregate.formats_shared.comment_reports;
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
    private ICollection<TestCommentReport> _commentReports { get; init; }
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
        _commentReports = [];
        _ratings = [];
    }

    public ImmutableHashSet<TestCommentId> CommentIds => _commentIds.ToImmutableHashSet();

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
        await testCommentsRepository.Update(parentComment);
        return answerCreationRes.GetSuccess();
    }

    public ErrOr<TestRating> AddRating(AppUserId userId, ushort ratingValue, IDateTimeProvider dateTimeProvider) {
        TestRating? existing = _ratings.FirstOrDefault(r => r.UserId == userId);
        if (existing is not null) {
            return new Err(
                $"This user has already rated this test. Current rating: {existing.Value}",
                details: $"Rating creation time: {existing.CreatedAt.ToShortDateString()}"
            );
        }

        var ratingCreationRes = TestRating.CreateNew(ratingValue, userId, Id, dateTimeProvider);
        if (ratingCreationRes.IsErr(out var err)) {
            return err;
        }

        TestRating rating = ratingCreationRes.GetSuccess();
        _ratings.Add(rating);
        _domainEvents.Add(new AppUserLeftTestRatingEvent(userId, rating.Id));
        return rating;
    }

    public ErrOrNothing UpdateRating(AppUserId userId, ushort ratingValue, IDateTimeProvider dateTimeProvider) {
        TestRating? existing = _ratings.FirstOrDefault(r => r.UserId == userId);
        if (existing is null) {
            return Err.ErrFactory.NotFound($"Cannot update rating because user has not rated this test");
        }

        var updatingRes = existing.Update(ratingValue, dateTimeProvider);
        if (updatingRes.IsErr(out var err)) {
            return err;
        }

        return ErrOrNothing.Nothing;
    }

    private const ushort RatingsPackageSize = 100;

    public ErrOr<ImmutableArray<TestRating>> GetRatingsPackage(int package) {
        if (package < 0) {
            return Err.ErrFactory.InvalidData("Package number cannot be negative");
        }

        return _ratings
            .Skip(package * RatingsPackageSize)
            .Take(RatingsPackageSize)
            .ToImmutableArray();
    }

    public async Task<ErrOr<ImmutableArray<TestRating>>> GetFilteredRatingsPackage(
        AppUserId? viewerId,
        IUserFollowingsRepository userFollowingsRepository,
        ListTestRatingsFilter filter,
        int package
    ) {
        if (package < 0) {
            return Err.ErrFactory.InvalidData("Package number cannot be negative");
        }

        if (filter.ByUserFollowings != FilterTriState.Unset && viewerId is null) {
            return Err.ErrFactory.Unauthorized(
                "Couldn't filter ratings by user followings because user is not logged in",
                details: "Login in or set 'by user followings' filter field to unset"
            );
        }

        if (filter.ByUserFollowers != FilterTriState.Unset && viewerId is null) {
            return Err.ErrFactory.Unauthorized(
                "Couldn't filter ratings by user followers because user is not logged in",
                details: "Login in or set 'by user followers' filter field to unset"
            );
        }

        Func<Task<ImmutableHashSet<AppUserId>>> viewerFollowings = async () =>
            (await userFollowingsRepository.GetUserFollowings(viewerId)).ToImmutableHashSet();

        Func<Task<ImmutableHashSet<AppUserId>>> viewerFollowers = async () =>
            (await userFollowingsRepository.GetUserFollowers(viewerId)).ToImmutableHashSet();

        var filteredRatings = _ratings
            .WithRatingValueFilter(filter.MinRatingValue, filter.MaxRatingValue)
            .WithCreationDateFilter(filter.CreationDateFrom, filter.CreationDateTo)
            .WithUpdatingDateFilter(filter.LastUpdateDateFrom, filter.LastUpdateDateTo)
            .WithWereUpdatedFilter(filter.WereUpdated);

        filteredRatings = await TestRatingsQueryExtensions
            .WithByFollowingsFilter(filteredRatings, filter.ByUserFollowings, viewerFollowings);
        filteredRatings = await TestRatingsQueryExtensions
            .WithByFollowersFilter(filteredRatings, filter.ByUserFollowers, viewerFollowers);

        return filteredRatings
            .ApplySorting(filter.Sorting)
            .Skip(package * RatingsPackageSize)
            .Take(RatingsPackageSize)
            .ToImmutableArray();
    }

    public ErrOrNothing CheckUserAccessToManageTest(AppUserId userId) {
        if (userId != CreatorId) {
            return Err.ErrFactory.NoAccess("Only test creator has access to manage test");
        }

        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing AddCommentReport(
        AppUserId userId,
        TestCommentId commentId,
        string reportText,
        CommentReportReason reportReason
    ) {
        bool commentExists = _commentIds.Any(cId => cId == commentId);
        if (!commentExists) {
            return Err.ErrFactory.NotFound(
                "Cannot report comment because this test does have this comment",
                details: $"Comment with id {commentId} has not been found in the test with id {Id}"
            );
        }

        TestCommentReport report = TestCommentReport.CreateNew();

        _domainEvents.Add(new TestCommentReportedEvent(commentId));
    }
}