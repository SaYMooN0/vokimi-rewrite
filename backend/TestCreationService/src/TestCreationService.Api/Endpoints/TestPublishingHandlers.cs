using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.Tests.formats_shared.commands.publishing;

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