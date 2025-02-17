using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Api.Contracts.view_test.comments.view_response;

internal record class HiddenCommentViewResponse(
    string Id,
    string AuthorId,
    uint AnswersCount,
    DateTime CreatedAt,
    bool HadAttachment
) : ITestCommentDataViewResponse
{
    public static HiddenCommentViewResponse FromComment(TestComment comment) => new(
        comment.Id.ToString(),
        comment.AuthorId.ToString(),
        comment.CurrentAnswersCount,
        comment.CreatedAt,
        comment.HasAttachment
    );
}