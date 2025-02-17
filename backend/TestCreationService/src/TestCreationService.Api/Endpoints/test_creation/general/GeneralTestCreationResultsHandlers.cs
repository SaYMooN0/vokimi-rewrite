using ApiShared.extensions;
using ApiShared;
using MediatR;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.Tests.general_format.commands.results;
using TestCreationService.Api.Contracts.Tests.test_creation.general_format.results;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;

namespace TestCreationService.Api.Endpoints.test_creation.general;

internal static class GeneralTestCreationResultsHandlers
{
    internal static RouteGroupBuilder MapGeneralTestCreationResultsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired();

        group.MapGet("/listIdsWithNames", ListResultIdsWithNames);
        group.MapGet("/list", ListResults);
        group.MapPost("/createNew", CreateNewResult);
        return group;
    }
    private async static Task<IResult> ListResultIdsWithNames(
       HttpContext httpContext,
       ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        ListGeneralTestResultsIdsWithNamesCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            dictionary => Results.Json(new {
                Results = dictionary.ToDictionary(
                    idWithName => idWithName.Key.ToString(),
                    idWithName => idWithName.Value
                )
            })
        );
    }
    private async static Task<IResult> ListResults(
       HttpContext httpContext,
       ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        ListResultsForGeneralTestCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            results => Results.Json(new {
                Results = results.Select(GeneralFormatTestResultInfoResponse.FromResult)
            })
        );
    }
    private async static Task<IResult> CreateNewResult(
       HttpContext httpContext,
       ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        CreateResultForGeneralTestCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            newResult => Results.Json(new {
                Result = GeneralFormatTestResultInfoResponse.FromResult(newResult)
            })
        );
    }
}