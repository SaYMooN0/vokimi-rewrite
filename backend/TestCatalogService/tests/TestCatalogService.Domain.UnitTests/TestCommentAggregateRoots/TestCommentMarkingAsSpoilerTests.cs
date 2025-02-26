using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.TestCommentAggregate;
using TestCatalogService.Domain.UnitTests.FakeRepositories;

namespace TestCatalogService.Domain.UnitTests.TestCommentAggregateRoots;

public class TestCommentMarkingAsSpoilerTests
{
    [Fact]
    public async Task MarkAsSpoiler_ByCommentAuthor_ShouldSucceed() {
        // Arrange
        var authorId = AppUserId.CreateNew();
        var comment = TestComment.CreateNew(
            TestCommentsTestsConsts.TestId,
            authorId, // Автор комментария
            TestCommentsTestsConsts.DefaultCommentText,
            attachment: null,
            markAsSpoiler: false,
            TestsSharedConsts.DateTimeProviderInstance
        ).GetSuccess();

        FakeBaseTestsRepository baseTestsRepositoryMock = new();

        // Act
        var markAsSpoilerRes = await comment.MarkAsSpoiler(authorId, baseTestsRepositoryMock);

        // Assert
        Assert.False(markAsSpoilerRes.IsErr());
        Assert.True(comment.MarkedAsSpoiler);
    }

    [Fact]
    public async Task MarkAsSpoiler_ByTestCreator_ShouldSucceed() {
        // Arrange
        var testCreatorId = AppUserId.CreateNew();
        var commentAuthorId = AppUserId.CreateNew();
        var comment = TestComment.CreateNew(
            TestCommentsTestsConsts.TestId,
            commentAuthorId, 
            TestCommentsTestsConsts.DefaultCommentText,
            attachment: null,
            markAsSpoiler: false,
            TestsSharedConsts.DateTimeProviderInstance
        ).GetSuccess();

        var baseTestsRepositoryMock = new FakeBaseTestsRepository() {
            GetTestCreatorIdFunc = (testId) => Task.FromResult<ErrOr<AppUserId>>(testCreatorId)
        };

        // Act
        var markAsSpoilerRes = await comment.MarkAsSpoiler(testCreatorId, baseTestsRepositoryMock);

        // Assert
        Assert.False(markAsSpoilerRes.IsErr());
        Assert.True(comment.MarkedAsSpoiler);
    }

    [Fact]
    public async Task MarkAsSpoiler_ByNonAuthorNonCreator_ShouldFail() {
        // Arrange
        var randomUserId = AppUserId.CreateNew();
        var commentAuthorId = AppUserId.CreateNew();
        var comment = TestComment.CreateNew(
            TestCommentsTestsConsts.TestId,
            commentAuthorId,
            TestCommentsTestsConsts.DefaultCommentText,
            attachment: null,
            markAsSpoiler: false,
            TestsSharedConsts.DateTimeProviderInstance
        ).GetSuccess();

        var baseTestsRepositoryMock = new FakeBaseTestsRepository() {
            GetTestCreatorIdFunc = (testId) => Task.FromResult<ErrOr<AppUserId>>(AppUserId.CreateNew())
        };

        // Act
        var markAsSpoilerRes = await comment.MarkAsSpoiler(randomUserId, baseTestsRepositoryMock);

        // Assert
        Assert.True(markAsSpoilerRes.IsErr(out var err));
        Assert.Equal(
            "You don't have permission to mark this comment as spoiler. You must be either the author of the comment or creator of the test",
            err.Message
        );
        Assert.False(comment.MarkedAsSpoiler);
    }

    [Fact]
    public async Task MarkAsSpoiler_FailToGetTestCreatorId_ShouldReturnError() {
        // Arrange
        var userId = AppUserId.CreateNew();
        var commentAuthorId = AppUserId.CreateNew();
        var comment = TestComment.CreateNew(
            TestCommentsTestsConsts.TestId,
            commentAuthorId,
            TestCommentsTestsConsts.DefaultCommentText,
            attachment: null,
            markAsSpoiler: false,
            TestsSharedConsts.DateTimeProviderInstance
        ).GetSuccess();

        var baseTestsRepositoryMock = new FakeBaseTestsRepository() {
            GetTestCreatorIdFunc = (testId) => Task.FromResult<ErrOr<AppUserId>>(Err.ErrFactory.NotFound("Not found"))
        };

        // Act
        var markAsSpoilerRes = await comment.MarkAsSpoiler(userId, baseTestsRepositoryMock);

        // Assert
        Assert.True(markAsSpoilerRes.IsErr());
        Assert.True(markAsSpoilerRes.IsErr(out var err));
        Assert.False(comment.MarkedAsSpoiler);
    }
}