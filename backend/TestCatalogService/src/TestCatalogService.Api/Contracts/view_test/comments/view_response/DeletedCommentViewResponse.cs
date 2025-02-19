using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Api.Contracts.view_test.comments.view_response;

internal record class DeletedCommentViewResponse(
    string Id,
    string AuthorId,
    uint AnswersCount,
    DateTime CreatedAt,
    UserCommentVoteState ViewerVote,
    uint UpVotesCount,
    uint DownVotesCount,
    bool HadAttachment
) : ITestCommentDataViewResponse
{
    public static DeletedCommentViewResponse FromComment(TestComment comment, UserCommentVoteState userVote) => new(
        comment.Id.ToString(),
        comment.AuthorId.ToString(),
        comment.CurrentAnswersCount,
        comment.CreatedAt,
        userVote,
        comment.UpVotesCount,
        comment.DownVotesCount,
        comment.HasAttachment
    );
}