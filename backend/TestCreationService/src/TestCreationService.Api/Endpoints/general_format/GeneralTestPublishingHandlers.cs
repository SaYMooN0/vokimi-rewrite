using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.Tests.general_format.commands.publishing;

namespace TestCreationService.Api.Endpoints.general_format;

internal static class GeneralTestPublishingHandlers
{
    internal static RouteGroupBuilder MapGeneralTestPublishingHandlers(this RouteGroupBuilder group) {
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
        CheckGeneralTestForPublishingProblemsCommand command = new(testId);
        var result = await mediator.Send(command);

        return Results.Json(new { TestPublishingProblems = result });
    }

    private static async Task<IResult> PublishTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        PublishGeneralTestCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(result, () => Results.Ok());
    }
}