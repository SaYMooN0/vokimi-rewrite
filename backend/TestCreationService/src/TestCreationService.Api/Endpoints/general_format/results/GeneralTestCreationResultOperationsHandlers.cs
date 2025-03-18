using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestCreationService.Api.Contracts.Tests.test_creation.general_format.results;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.Tests.general_format.commands.results;

namespace TestCreationService.Api.Endpoints.general_format.results;

internal static class GeneralTestCreationResultOperationHandlers
{
    internal static RouteGroupBuilder MapGeneralTestCreationResultOperationHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired();

        group.MapDelete("/delete", DeleteResult);
        group.MapPost("/update", UpdateResult)
            .WithRequestValidation<UpdateGeneralTestResultRequest>();
        return group;
    }
    private async static Task<IResult> DeleteResult(
       HttpContext httpContext,
       ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        GeneralTestResultId resultId = httpContext.GetGeneralTestResultIdFromRoute();

        DeleteGeneralTestResultCommand command = new(testId, resultId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
        );
    }
    private async static Task<IResult> UpdateResult(
        HttpContext httpContext,
        ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        GeneralTestResultId resultId = httpContext.GetGeneralTestResultIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateGeneralTestResultRequest>();

        UpdateGeneralTestResultCommand command = new(testId, resultId, request.Name, request.Text, request.Image);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
        );
    }
}