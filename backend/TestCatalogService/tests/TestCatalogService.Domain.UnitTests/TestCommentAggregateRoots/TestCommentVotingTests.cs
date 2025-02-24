using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Domain.UnitTests.TestCommentAggregateRoots;

public class TestCommentVotingTests
{
    [Fact]
    public void Vote_ForNewComment_ShouldAddVote() {
        // Arrange
        var userId = AppUserId.CreateNew();
        var comment = TestComment.CreateNew(
            TestCommentsTestsConsts.TestId,
            TestCommentsTestsConsts.AuthorId,
            TestCommentsTestsConsts.DefaultCommentText,
            attachment: null,
            markAsSpoiler: false,
            TestCommentsTestsConsts.DateTimeProviderInstance
        ).GetSuccess();

        // Act
        var voteRes = comment.Vote(userId, true); 

        // Assert
        Assert.True(!voteRes.IsErr());
        Assert.Equal(UserCommentVoteState.Up, voteRes.GetSuccess());
        Assert.Equal((uint)1, comment.UpVotesCount);
        Assert.Equal((uint)0, comment.DownVotesCount);
    }
    [Fact]
    public void Vote_SameVote_ShouldRemoveVote() {
        // Arrange
        var userId = AppUserId.CreateNew();
        var comment = TestComment.CreateNew(
            TestCommentsTestsConsts.TestId,
            TestCommentsTestsConsts.AuthorId,
            TestCommentsTestsConsts.DefaultCommentText,
            attachment: null,
            markAsSpoiler: false,
            TestCommentsTestsConsts.DateTimeProviderInstance
        ).GetSuccess();
        comment.Vote(userId, true);

        // Act
        var voteRes = comment.Vote(userId, true);

        // Assert
        Assert.True(!voteRes.IsErr());
        Assert.Equal(UserCommentVoteState.None, voteRes.GetSuccess());
        Assert.Equal((uint)0, comment.UpVotesCount);
        Assert.Equal((uint)0, comment.DownVotesCount);
    }
    [Fact]
    public void Vote_ChangeVoteFromUpToDown_ShouldUpdateVotes() {
        // Arrange
        var userId = AppUserId.CreateNew();
        var comment = TestComment.CreateNew(
            TestCommentsTestsConsts.TestId,
            TestCommentsTestsConsts.AuthorId,
            TestCommentsTestsConsts.DefaultCommentText,
            attachment: null,
            markAsSpoiler: false,
            TestCommentsTestsConsts.DateTimeProviderInstance
        ).GetSuccess();
        comment.Vote(userId, true); 

        // Act
        var voteRes = comment.Vote(userId, false);

        // Assert
        Assert.True(!voteRes.IsErr());
        Assert.Equal(UserCommentVoteState.Down, voteRes.GetSuccess());
        Assert.Equal((uint)0, comment.UpVotesCount);
        Assert.Equal((uint)1, comment.DownVotesCount);
    }
    [Fact]
    public void Vote_ChangeVoteFromDownToUp_ShouldUpdateVotes() {
        // Arrange
        var userId = AppUserId.CreateNew();
        var comment = TestComment.CreateNew(
            TestCommentsTestsConsts.TestId,
            TestCommentsTestsConsts.AuthorId,
            TestCommentsTestsConsts.DefaultCommentText,
            attachment: null,
            markAsSpoiler: false,
            TestCommentsTestsConsts.DateTimeProviderInstance
        ).GetSuccess();
        comment.Vote(userId, false);

        // Act
        var voteRes = comment.Vote(userId, true);

        // Assert
        Assert.True(!voteRes.IsErr());
        Assert.Equal(UserCommentVoteState.Up, voteRes.GetSuccess());
        Assert.Equal((uint)1, comment.UpVotesCount);
        Assert.Equal((uint)0, comment.DownVotesCount);
    }
}