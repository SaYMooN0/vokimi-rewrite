using SharedKernel.Common.errors;
using TestCatalogService.Domain.Rules;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Domain.UnitTests.TestCommentAggregateRoots;

public class TestCommentCreationTests
{
    [Fact]
    public void CreateNewComment_WithCorrectDataWithoutAttachment_ShouldSucceed() {
        //Act
        var creationRes = TestComment.CreateNew(
            TestCommentsTestsConsts.TestId,
            TestCommentsTestsConsts.AuthorId,
            TestCommentsTestsConsts.DefaultCommentText,
            attachment: null,
            markAsSpoiler: false,
            TestsSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(creationRes.IsSuccess(out var comment));
        Assert.Equal(TestCommentsTestsConsts.TestId, comment.TestId);
        Assert.Equal(TestCommentsTestsConsts.AuthorId, comment.AuthorId);
        Assert.Equal(TestCommentsTestsConsts.DefaultCommentText, comment.Text.GetSuccess());
        Assert.Null(comment.Attachment.GetSuccess());
        Assert.Equal(TestCommentsTestsConsts.DefaultMarkAsSpoiler, comment.MarkedAsSpoiler);
        Assert.Equal((uint)0, comment.CurrentAnswersCount);
        Assert.Equal((uint)0, comment.UpVotesCount);
        Assert.Equal((uint)0, comment.DownVotesCount);
        Assert.False(comment.IsHidden);
        Assert.False(comment.IsDeleted);
        Assert.Null(comment.DeletedAt);
    }

    [Fact]
    public void CreateNewComment_MarkedAsSpoiler_ShouldCreateMarkedAsSpoilerComment() {
        //Act
        var creationRes = TestComment.CreateNew(
            TestCommentsTestsConsts.TestId,
            TestCommentsTestsConsts.AuthorId,
            TestCommentsTestsConsts.DefaultCommentText,
            attachment: null,
            markAsSpoiler: true,
            TestsSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(creationRes.IsSuccess(out var comment));
        Assert.True(comment.MarkedAsSpoiler);
        Assert.False(comment.IsHidden);
        Assert.False(comment.IsDeleted);
        Assert.Null(comment.DeletedAt);
    }

    [Fact]
    public void CreateNewComment_WithTooLongTest_ShouldReturnErr() {
        //Act
        var creationRes = TestComment.CreateNew(
            TestCommentsTestsConsts.TestId,
            TestCommentsTestsConsts.AuthorId,
            new string('a', TestCommentRules.MaxCommentLength + 1),
            attachment: null,
            markAsSpoiler: true,
            TestsSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(creationRes.IsErr(out var err));
        Assert.Equal(err.Code, Err.ErrCodes.InvalidData);
    }
}
