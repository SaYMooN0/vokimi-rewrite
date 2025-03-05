using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Api.Contracts.view_test;
using TestCatalogService.Api.Extensions;
using TestCatalogService.Application.Tests.formats_shared.commands;

namespace TestCatalogService.Api.Endpoints.view_test;

internal static class ViewTestRootHandlers
{
    internal static RouteGroupBuilder MapViewTestRootHandlers(this RouteGroupBuilder group) {
        group
            .GroupUserAccessToViewTestRequired();

        group.MapGet("/", LoadViewTestData);
        
        return group;
    }

    private static async Task<IResult> LoadViewTestData(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        LoadViewTestDataCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            (test) => Results.Json(ViewTestLoadDataResponse.Create(test))
        );
    }
}