using System.Text.Json;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Api.Contracts.view_test.comments.view_response;

internal record class TestCommentViewDataResponse(
    string Id,
    string AuthorId,
    uint AnswersCount,
    DateTime CreatedAt,
    UserCommentVoteState ViewerVote,
    uint UpVotesCount,
    uint DownVotesCount,
    bool MarkedAsSpoiler,
    string Text,
    string? AttachmentJson,
    bool WasEdited
) : ITestCommentDataViewResponse
{
    public static TestCommentViewDataResponse FromComment(TestComment comment, UserCommentVoteState userVote) => new(
        comment.Id.ToString(),
        comment.AuthorId.ToString(),
        comment.CurrentAnswersCount,
        comment.CreatedAt,
        userVote,
        comment.UpVotesCount,
        comment.DownVotesCount,
        comment.MarkedAsSpoiler,
        comment.Text.GetSuccess(),
        comment.Attachment.GetSuccess()?.ToStorageString() ?? null,
        comment.LastEditTime is not null
    );

    private static string? GetAttachmentJson(TestCommentAttachment? attachment) =>
        attachment is null ? null : JsonSerializer.Serialize(attachment);
}