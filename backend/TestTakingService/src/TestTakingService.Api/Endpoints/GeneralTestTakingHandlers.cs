using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain;
using TestTakingService.Application.Tests.general_format.commands;

namespace TestTakingService.Api.Endpoints;

public static class GeneralTestTakingHandlers
{
    internal static RouteGroupBuilder MapGeneralTestTakingHandlers(this RouteGroupBuilder group) {
        //group map check access
        group.MapGet("/loadTestTakingData", LoadGeneralTestTakingData);
        return group;
    }

    private async static Task<IResult> LoadGeneralTestTakingData(
        HttpContext httpContext,
        ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        LoadGeneralTestTakingDataCommand command = new(testId);
        var result = await mediator.Send(command);

        return Results.Json(new { });
    }
}