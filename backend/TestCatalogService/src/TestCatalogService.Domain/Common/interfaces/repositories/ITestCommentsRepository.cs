using System.Collections.Immutable;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.Common.filters;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Domain.Common.interfaces.repositories;

public interface ITestCommentsRepository
{
    public Task Add(TestComment comment);
    public Task<TestComment?> GetById(TestCommentId commentId);
    public Task Update(TestComment comment);

    public Task<ImmutableArray<TestCommentWithViewerVote>> GetCommentsPackageForViewer(
        TestId testId,
        uint packageNumber,
        AppUserId? viewer
    );

    public Task<ImmutableArray<TestCommentWithViewerVote>> GetFilteredCommentsPackageForViewer(
        TestId testId,
        uint packageNumber,
        AppUserId? viewer,
        ListTestCommentsFilter filter
    );
}

public record TestCommentWithViewerVote(TestComment Comment, UserCommentVoteState UserVote)
{
    public static TestCommentWithViewerVote Create(TestComment comment, CommentVote? userVote) {
        UserCommentVoteState voteState = userVote is null
            ? UserCommentVoteState.None
            : userVote.IsUp
                ? UserCommentVoteState.Up
                : UserCommentVoteState.Down;
        return new TestCommentWithViewerVote(comment, voteState);
    }
}