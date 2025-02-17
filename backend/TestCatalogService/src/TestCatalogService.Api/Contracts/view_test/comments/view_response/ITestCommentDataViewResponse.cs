using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Api.Contracts.view_test.comments.view_response;

internal interface ITestCommentDataViewResponse
{
    public string Id { get; init; }
    public string AuthorId { get; init; }
    public uint AnswersCount { get; init; }
    public DateTime CreatedAt { get; init; }

} 
