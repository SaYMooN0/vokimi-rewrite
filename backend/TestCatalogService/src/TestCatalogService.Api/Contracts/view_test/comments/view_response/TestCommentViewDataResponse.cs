using System.Text.Json;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Api.Contracts.view_test.comments.view_response;

internal record class TestCommentViewDataResponse(
    string Id,
    string AuthorId,
    uint AnswersCount,
    DateTime CreatedAt,
    bool? ViewerVoteValue,
    uint UpVotesCount,
    uint DownVotesCount,
    bool MarkedAsSpoiler,
    string Text,
    string? AttachmentJson
) : ITestCommentDataViewResponse
{
    public static TestCommentViewDataResponse FromComment(TestComment comment, bool? userVoteValue) => new(
        comment.Id.ToString(),
        comment.AuthorId.ToString(),
        comment.CurrentAnswersCount,
        comment.CreatedAt,
        userVoteValue,
        comment.UpVotesCount,
        comment.DownVotesCount,
        comment.MarkedAsSpoiler,
        comment.Text.GetSuccess(),
        comment.Attachment.GetSuccess()?.ToStorageString() ?? null
    );

    private static string? GetAttachmentJson(TestCommentAttachment? attachment) =>
        attachment is null ? null : JsonSerializer.Serialize(attachment);
}