using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Configs;
using TestTakingService.Api.Contracts.general_format_test.load_test_taking_data;
using TestTakingService.Api.Contracts.general_format_test.test_taken;
using TestTakingService.Api.Extensions;
using TestTakingService.Application.Tests.general_format.commands;

namespace TestTakingService.Api.Endpoints;

public static class GeneralTestTakingHandlers
{
    internal static RouteGroupBuilder MapGeneralTestTakingHandlers(this RouteGroupBuilder group) {
        group
            .GroupUserAccessToTakeTestRequired();

        group.MapGet("/loadTestTakingData", LoadGeneralTestTakingData);
        group.MapPost("/testTaken", HandleGeneralTestTaken)
            .WithRequestValidation<GeneralTestTakenRequest>();
        return group;
    }

    private static async Task<IResult> LoadGeneralTestTakingData(
        HttpContext httpContext,
        ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        LoadGeneralTestTakingDataCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            (test) => Results.Json(GeneralTestTakingDataResponse.FromTest(test))
        );
    }

    private static async Task<IResult> HandleGeneralTestTaken(
        HttpContext httpContext,
        ISender mediator,
        JwtTokenConfig jwtConfig
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        AppUserId? userId = httpContext.ParseUserIdFromJwtToken(jwtConfig).IsSuccess(out var userIdVal)
            ? userIdVal
            : null;
        GeneralTestTakenCommand command = new(testId);
    }
}