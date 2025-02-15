using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using TestCatalogService.Api.Contracts.view_test;
using TestCatalogService.Api.Extensions;

namespace TestCatalogService.Api.Endpoints.view_test;

internal  static class ViewTestRootHandlers
{
    internal static RouteGroupBuilder MapViewTestRootHandlers(this RouteGroupBuilder group) {
        group
            .GroupUserAccessToViewTestRequired();

        group.MapGet("/", LoadViewTestData);
        
        return group;
    }

    private async static Task<IResult> LoadViewTestData(
        HttpContext httpContext,
        ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        LoadViewTestDataCommand command = new(testId);
        var result = await mediator.Send(command);

        return Results.Json(ViewTestLoadDataResponse.Create());
    }
}