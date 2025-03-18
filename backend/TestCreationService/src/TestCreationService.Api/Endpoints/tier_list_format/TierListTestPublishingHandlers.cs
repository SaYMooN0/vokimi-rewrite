using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.Tests.tier_list_format.commands.publishing;

namespace TestCreationService.Api.Endpoints.tier_list_format;

internal static class TierListTestPublishingHandlers
{
    internal static RouteGroupBuilder MapTierListTestPublishingHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired();

        group.MapPost("/checkForProblems", CheckForPublishingProblems)
            .TestEditPermissionRequired();

        group.MapPost("/publish", PublishTest)
            .OnlyByTestCreator();
        return group;
    }

    private static async Task<IResult> CheckForPublishingProblems(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        CheckTierListTestForPublishingProblemsCommand command = new(testId);
        var result = await mediator.Send(command);

        return Results.Json(new { TestPublishingProblems = result });
    }

    private static async Task<IResult> PublishTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        PublishTierListTestCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(result, () => Results.Ok());
    }
}