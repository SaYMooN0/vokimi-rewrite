using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Domain.UnitTests.TestCommentAggregateRoots;

public class TestCommentDeletingTests
{
    [Fact]
    public void DeleteComment_ThatWasNotDeletedBefore_ShouldSucceed() {
        // Arrange
        TestComment comment = TestComment.CreateNew(
            TestCommentsTestsConsts.TestId,
            TestCommentsTestsConsts.AuthorId,
            TestCommentsTestsConsts.DefaultCommentText,
            attachment: null,
            markAsSpoiler: false,
            TestCommentsTestsConsts.DateTimeProviderInstance
        ).GetSuccess();

        // Act
        var deleteRes = comment.Delete(
            TestCommentsTestsConsts.AuthorId,
            TestCommentsTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(!deleteRes.IsErr());
        Assert.True(comment.IsDeleted);
        Assert.NotNull(comment.DeletedAt);
        Assert.True(comment.Text.IsErr(out var textAccessErr));
        Assert.Equal("Cannot access deleted comment text", textAccessErr.Message);
        Assert.True(comment.Attachment.IsErr(out var attachmentAccessErr));
        Assert.Equal("Cannot access deleted comment attachment", attachmentAccessErr.Message);
    }

    [Fact]
    public void DeleteComment_ThatWasAlreadyDeleted_ShouldReturnError() {
        // Arrange
        TestComment comment = TestComment.CreateNew(
            TestCommentsTestsConsts.TestId,
            TestCommentsTestsConsts.AuthorId,
            TestCommentsTestsConsts.DefaultCommentText,
            attachment: null,
            markAsSpoiler: false,
            TestCommentsTestsConsts.DateTimeProviderInstance
        ).GetSuccess();

        // Act
        var deleteRes1 = comment.Delete(
            TestCommentsTestsConsts.AuthorId,
            TestCommentsTestsConsts.DateTimeProviderInstance
        );
        var deleteRes2 = comment.Delete(
            TestCommentsTestsConsts.AuthorId,
            TestCommentsTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(!deleteRes1.IsErr()); // First delete should succeed
        Assert.True(deleteRes2.IsErr(out var err)); // Second delete should fail
        Assert.Equal("This comment is already deleted", err.Message);
        Assert.True(comment.IsDeleted);
    }


    [Fact]
    public void DeleteComment_WithAnswers_ShouldNotDeleteAnswers() {
        // Arrange
        TestComment comment = TestComment.CreateNew(
            TestCommentsTestsConsts.TestId,
            TestCommentsTestsConsts.AuthorId,
            TestCommentsTestsConsts.DefaultCommentText,
            attachment: null,
            markAsSpoiler: false,
            TestCommentsTestsConsts.DateTimeProviderInstance
        ).GetSuccess();
    
        TestComment answer = TestComment.CreateNew(
            TestCommentsTestsConsts.TestId,
            TestCommentsTestsConsts.AuthorId,
            "This is an answer",
            attachment: null,
            markAsSpoiler: false,
            TestCommentsTestsConsts.DateTimeProviderInstance
        ).GetSuccess();
        comment.AddAnswer(answer);

        // Act
        var deleteRes = comment.Delete(TestCommentsTestsConsts.AuthorId, TestCommentsTestsConsts.DateTimeProviderInstance);

        // Assert
        Assert.True(!deleteRes.IsErr()); 
        Assert.True(comment.IsDeleted);
        Assert.Contains(answer, comment.Answers); // The answer should still be there
    }
    [Fact]
    public void DeleteComment_WithoutAuthentication_ShouldReturnError() {
        // Arrange
        TestComment comment = TestComment.CreateNew(
            TestCommentsTestsConsts.TestId,
            TestCommentsTestsConsts.AuthorId,
            TestCommentsTestsConsts.DefaultCommentText,
            attachment: null,
            markAsSpoiler: false,
            TestCommentsTestsConsts.DateTimeProviderInstance
        ).GetSuccess();

        // Act
        var deleteRes = comment.Delete(null, TestCommentsTestsConsts.DateTimeProviderInstance);

        // Assert
        Assert.True(deleteRes.IsErr(out var err));
        Assert.Equal("To delete comment you need to be authenticated and be the author of the comment", err.Message);
    }
    [Fact]
    public void DeleteComment_AsDifferentUser_ShouldReturnError() {
        // Arrange
        AppUserId otherUserId = AppUserId.CreateNew(); // Simulate a different user
        TestComment comment = TestComment.CreateNew(
            TestCommentsTestsConsts.TestId,
            TestCommentsTestsConsts.AuthorId,
            TestCommentsTestsConsts.DefaultCommentText,
            attachment: null,
            markAsSpoiler: false,
            TestCommentsTestsConsts.DateTimeProviderInstance
        ).GetSuccess();

        // Act
        var deleteRes = comment.Delete(otherUserId, TestCommentsTestsConsts.DateTimeProviderInstance);

        // Assert
        Assert.True(deleteRes.IsErr(out var err));
        Assert.Equal("To delete comment you must be the author of the comment", err.Message);
    }

}