namespace TestCatalogService.Api.Contracts.view_test.comments;

internal record class CommentViewDataResponse(
    string Id,
    string AuthorId,
    uint AnswersCount,
    uint UpVotesCount,
    uint DownVotesCount,
    int VotesRating,
    string Text,
    Dictionary<string, string> Attachment,
    DateTime CreatedAt
) { }