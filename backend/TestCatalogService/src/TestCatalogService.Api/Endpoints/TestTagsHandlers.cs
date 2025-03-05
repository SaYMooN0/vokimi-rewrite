using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Application.TestTags.commands;

namespace TestCatalogService.Api.Endpoints;


internal static class TestTagsHandlers
{
    internal static RouteGroupBuilder MapTestTagsHandlers(this RouteGroupBuilder group) {
        group.MapGet("/search/{tag}", SearchTagsToSuggestForTest);

        return group;
    }
    private static async Task<IResult> SearchTagsToSuggestForTest(
        HttpContext httpContext, ISender mediator, string tag
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();


        SearchTagsCommand command = new(tag);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            (tags) => Results.Json(new { Tags = tags })
        );
    }
}