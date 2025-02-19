using ApiShared;
using ApiShared.extensions;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCatalogService.Api.Extensions;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.Common.interfaces.repositories;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Api.EndpointsFilters;

public class CheckCommentBelongsToTestEndpointFilter : IEndpointFilter
{
    private readonly ITestCommentsRepository _testCommentsRepository;

    public CheckCommentBelongsToTestEndpointFilter(ITestCommentsRepository testCommentsRepository) {
        _testCommentsRepository = testCommentsRepository;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
        var commentIdRes = context.HttpContext.GetCommentTestIdFromRoute();
        if (commentIdRes.IsErr(out var err)) {
            return CustomResults.ErrorResponse(err);
        }

        TestCommentId id = commentIdRes.GetSuccess();
        TestComment? comment = await _testCommentsRepository.GetById(id);
        if (comment is null) {
            return TestCatalogErrPresets.CommentNotFound(id);
        }

        TestId routeTestId = context.HttpContext.GetTestIdFromRoute();
        if (comment.TestId != routeTestId) {
            return new Err(
                "Incorrect comment with test relations. Comment doesn't belong to this test",
                details: $"Comment with id {comment.Id} does not belong to test with id {routeTestId}"
            );
        }

        return await next(context);
    }
}