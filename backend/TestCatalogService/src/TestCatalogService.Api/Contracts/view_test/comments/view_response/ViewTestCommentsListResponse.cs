using System.Collections.Immutable;
using TestCatalogService.Application.Common.interfaces.repositories;

namespace TestCatalogService.Api.Contracts.view_test.comments.view_response;

internal record class ViewTestCommentsListResponse(ITestCommentDataViewResponse[] Comments)
{
    public static ViewTestCommentsListResponse FromCommentsWithVotes(
        ImmutableArray<TestCommentWithViewerVote> comments
    ) => new(comments.Select(CommentToViewData).ToArray());

    private static ITestCommentDataViewResponse CommentToViewData(TestCommentWithViewerVote commentWithVote) {
        var comment = commentWithVote.Comment;
        if (comment.IsDeleted) {
            return DeletedCommentViewResponse.FromComment(comment, commentWithVote.UserVote);
        }

        if (comment.IsHidden) {
            return HiddenCommentViewResponse.FromComment(comment);
        }

        return TestCommentViewDataResponse.FromComment(comment, commentWithVote.UserVote);
    }
}