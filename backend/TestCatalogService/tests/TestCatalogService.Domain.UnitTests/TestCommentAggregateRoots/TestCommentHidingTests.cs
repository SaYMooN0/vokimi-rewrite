using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate;
using TestCatalogService.Domain.TestCommentAggregate;
using TestCatalogService.Domain.UnitTests.FakeRepositories;

namespace TestCatalogService.Domain.UnitTests.TestCommentAggregateRoots;

public class TestCommentHidingTests
{
    [Fact]
    public async Task HideComment_ByTestCreator_ShouldSucceed() {
        // Arrange
        var testId = TestCommentsTestsConsts.TestId;
        var creatorId = TestCommentsTestsConsts.AuthorId; // Создатель теста
        var comment = TestComment.CreateNew(
            testId,
            creatorId,
            TestCommentsTestsConsts.DefaultCommentText,
            attachment: null,
            markAsSpoiler: false,
            TestCommentsTestsConsts.DateTimeProviderInstance
        ).GetSuccess();

        var baseTestsRepositoryMock = new FakeBaseTestsRepository {
            GetTestCreatorIdFunc = (testId) => Task.FromResult<ErrOr<AppUserId>>(creatorId)
        };

        // Act
        var hideRes = await comment.Hide(creatorId, baseTestsRepositoryMock);

        // Assert
        Assert.True(!hideRes.IsErr());
        Assert.True(comment.IsHidden); 
    }
    [Fact]
    public async Task HideComment_ByNonCreator_ShouldReturnError() {
        // Arrange
        var testId = TestCommentsTestsConsts.TestId;
        var creatorId = TestCommentsTestsConsts.AuthorId;
        var nonCreatorId =  AppUserId.CreateNew(); 
        var comment = TestComment.CreateNew(
            testId,
            creatorId,
            TestCommentsTestsConsts.DefaultCommentText,
            attachment: null,
            markAsSpoiler: false,
            TestCommentsTestsConsts.DateTimeProviderInstance
        ).GetSuccess();

        var baseTestsRepositoryMock = new FakeBaseTestsRepository {
            GetTestCreatorIdFunc = (testId) => Task.FromResult<ErrOr<AppUserId>>(creatorId)
        };

        // Act
        var hideRes = await comment.Hide(nonCreatorId, baseTestsRepositoryMock);

        // Assert
        Assert.True(hideRes.IsErr(out var err));
        Assert.Equal("You don't have permission to hide this comment as spoiler. To hide comments you must bet the test creator", err.Message);
        Assert.False(comment.IsHidden); 
    }
    [Fact]
    public async Task HideComment_WhenFetchingCreatorFails_ShouldReturnError() {
        // Arrange
        var testId = TestCommentsTestsConsts.TestId;
        var creatorId = TestCommentsTestsConsts.AuthorId;
        var comment = TestComment.CreateNew(
            testId,
            creatorId,
            TestCommentsTestsConsts.DefaultCommentText,
            attachment: null,
            markAsSpoiler: false,
            TestCommentsTestsConsts.DateTimeProviderInstance
        ).GetSuccess();

        var baseTestsRepositoryMock = new FakeBaseTestsRepository {
            GetTestCreatorIdFunc = (testId) => Task.FromResult<ErrOr<AppUserId>>(new Err("Error fetching test creator"))
        };

        // Act
        var hideRes = await comment.Hide(creatorId, baseTestsRepositoryMock);

        // Assert
        Assert.True(hideRes.IsErr(out var err));
        Assert.Equal("Error fetching test creator", err.Message);
        Assert.False(comment.IsHidden);
    }

}

