namespace TestCatalogService.Domain.UnitTests.TestCommentAggregateRoots;

public class TestCommentCreationTests
{
    [Fact]
    public void Should_CreateNewComment_Successfully_WithoutAttachment() {
        //Act
        var comment = TestCommentsTestsConsts.CreateNewComment(attachment: null);

        // Assert

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
}