using ApiShared.extensions;
using ApiShared;
using MediatR;
using TestCreationService.Api.Extensions;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;

namespace TestCreationService.Api.Endpoints;


public static class TestPublishingHandlers
{
    internal static RouteGroupBuilder MapTestPublishingHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired();

        group.MapPost("/checkForProblems", CheckTestForPublishingProblems)
            .TestEditPermissionRequired();

        group.MapPost("/publish", PublishTest)
            .OnlyByTestCreator();
        return group;
    }
    private async static Task<IResult> CheckTestForPublishingProblems(
        HttpContext httpContext,
        ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        CheckTestForPublishingProblemsCommand command = new(testId);
        var result = await mediator.Send(command);

        return Results.Json(new { TestPublishingProblems = result });
    }
    private async static Task<IResult> PublishTest(
        HttpContext httpContext,
        ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        PublishTestCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result, () => Results.Ok()
        );
    }
}