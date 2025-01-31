using ApiShared.extensions;
using ApiShared;
using MediatR;
using SharedKernel.Common.EntityIds;
using TestCreationService.Api.Contracts.Tests.test_initialization;
using TestCreationService.Application.Tests.general_format.commands;
using TestCreationService.Api.Extensions;
using OneOf.Types;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using TestCreationService.Api.Contracts.Tests.test_publishing;

namespace TestCreationService.Api.Endpoints;


public static class TestPublishingHandlers
{
    internal static RouteGroupBuilder MapTestPublishingHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired();

        group.MapPost("/checkForPublishingErrs", CheckTestForPublishingErrs)
            .TestEditPermissionRequired();

        group.MapPost("/publish", PublishTest)
            .OnlyByTestCreator();
        return group;
    }
    private async static Task<IResult> CheckTestForPublishingErrs(
        HttpContext httpContext,
        ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        CheckTestForPublishingErrsCommand command = new(testId);
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
            result,
            () => Results.Json(new TestPublishingResultResponse(), statusCode: 201)
        );
    }
}