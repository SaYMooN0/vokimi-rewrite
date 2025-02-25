using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Api.Contracts.view_test.comments;

public record class TestCommentDeletedResponse(
    uint AnswersCount,
    uint UpVotesCount,
    uint DownVotesCount,
    bool HadAttachment
)
{
    public static TestCommentDeletedResponse FromComment(TestComment comment) => new(
        comment.CurrentAnswersCount,
        comment.UpVotesCount,
        comment.DownVotesCount,
        comment.HasAttachment
    );
}