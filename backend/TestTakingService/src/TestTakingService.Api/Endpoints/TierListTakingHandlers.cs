using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Configs;
using TestTakingService.Api.Contracts.load_test_taking_data.general_test;
using TestTakingService.Api.Contracts.load_test_taking_data.tier_list_test;
using TestTakingService.Api.Contracts.test_taken.general_test;
using TestTakingService.Api.Contracts.test_taken.tier_list_test;
using TestTakingService.Api.Extensions;
using TestTakingService.Application.Tests.general_format.commands;
using TestTakingService.Application.Tests.tier_list_format.commands;

namespace TestTakingService.Api.Endpoints;

internal static class TierListTakingHandlers
{
    internal static RouteGroupBuilder MapTierListTakingHandlers(this RouteGroupBuilder group) {
        group
            .GroupUserAccessToTakeTestRequired();

        group.MapGet("/loadTestTakingData", LoadTestTakingData);
        group.MapPost("/testTaken", HandleTierListTestTaken)
            .WithRequestValidation<TierListTestTakenRequest>();

        return group;
    }

    private static async Task<IResult> LoadTestTakingData(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        LoadTierListTestTakingDataCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            (test) => Results.Json(TierListTestTakingDataResponse.FromTest(test))
        );
    }

    private static async Task<IResult> HandleTierListTestTaken(
        HttpContext httpContext, ISender mediator, JwtTokenConfig jwtConfig
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        AppUserId? userId = httpContext.ParseUserIdFromJwtToken(jwtConfig).IsSuccess(out var userIdVal)
            ? userIdVal
            : null;
        var request = httpContext.GetValidatedRequest<TierListTestTakenRequest>();

        TierListTestTakenCommand command = new(
            testId,
            userId,
            request.ParsedItemsInTiers,
            request.Feedback,
            TestTakingStart: request.StartDateTime,
            TestTakingEnd: request.EndDateTime
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result,
            (testTakingRes) => Results.Json(
                new { TestTakingResult = GeneralTestTakenReceivedResultResponse.FromResult(receivedRes) }
            )
        );
    }
}