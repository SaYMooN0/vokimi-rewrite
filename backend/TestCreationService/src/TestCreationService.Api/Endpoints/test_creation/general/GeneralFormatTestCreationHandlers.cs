using ApiShared.extensions;
using ApiShared;
using MediatR;
using TestCreationService.Application.Tests.general_format.commands;
using TestCreationService.Api.Contracts.Tests.test_creation.general_format;
using TestCreationService.Api.Extensions;

namespace TestCreationService.Api.Endpoints.test_creation.general;

internal static class GeneralFormatTestCreationHandlers
{
    internal static RouteGroupBuilder MapGeneralFormatTestCreationHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired();

        group
            .MapPost("/updateFeedbackOption", UpdateTestFeedbackOption)
            .WithRequestValidation<UpdateGeneralTestFeedbackOptionRequest>();
        return group;
    }

    private static async Task<IResult> UpdateTestFeedbackOption(
        HttpContext httpContext,
        ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<UpdateGeneralTestFeedbackOptionRequest>();
        var testId = httpContext.GetTestIdFromRoute();
        var feedbackOption = request.CreateFeedbackOption().GetSuccess();

        UpdateGeneralTestFeedbackCommand command = new(testId, feedbackOption);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
        );
    }
}