using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Api.Contracts.view_test.comments.view_response;

internal record class DeletedCommentViewResponse(
    string Id,
    string AuthorId,
    uint AnswersCount,
    DateTime CreatedAt,
    bool? ViewerVoteValue,
    uint UpVotesCount,
    uint DownVotesCount,
    bool HadAttachment
) : ITestCommentDataViewResponse
{
    public static DeletedCommentViewResponse FromComment(TestComment comment, bool? userVoteValue) => new(
        comment.Id.ToString(),
        comment.AuthorId.ToString(),
        comment.CurrentAnswersCount,
        comment.CreatedAt,
        userVoteValue,
        comment.UpVotesCount,
        comment.DownVotesCount,
        comment.HasAttachment
    );
}