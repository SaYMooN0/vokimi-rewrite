using ApiShared;
using ApiShared.extensions;
using MediatR;
using TestCreationService.Api.Contracts.Tests.test_creation.general_format.feedback_option;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.Tests.general_format.commands;

namespace TestCreationService.Api.Endpoints.test_creation.general_format;

internal static class GeneralFormatTestCreationHandlers
{
    internal static RouteGroupBuilder MapGeneralFormatTestCreationHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired();

        group.MapGet("/feedbackOptionInfo", GetTestFeedbackOption);
        group
            .MapPost("/updateFeedbackOption", UpdateTestFeedbackOption)
            .WithRequestValidation<UpdateGeneralTestFeedbackOptionRequest>();
        return group;
    }
    private static async Task<IResult> GetTestFeedbackOption(
        HttpContext httpContext, ISender mediator
    ) {
        var testId = httpContext.GetTestIdFromRoute();

        GetGeneralTestFeedbackOptionCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result, (option) => Results.Json(new {
                feedback = GeneralTestFeedbackOptionInfoResponse.FromFeedbackOption(option)
            })
        );
    }
    private static async Task<IResult> UpdateTestFeedbackOption(
        HttpContext httpContext, ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<UpdateGeneralTestFeedbackOptionRequest>();
        var testId = httpContext.GetTestIdFromRoute();
        var feedbackOption = request.CreateFeedbackOption().GetSuccess();

        UpdateGeneralTestFeedbackCommand command = new(testId, feedbackOption);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result, (option) => Results.Json(new {
                feedback = GeneralTestFeedbackOptionInfoResponse.FromFeedbackOption(option)
            })
        );
    }
}