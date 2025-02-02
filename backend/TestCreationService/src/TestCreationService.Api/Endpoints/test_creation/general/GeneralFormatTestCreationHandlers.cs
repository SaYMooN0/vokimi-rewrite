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
        group.MapPost("/updateTestTakingProcessSettings", UpdateTestTakingProcessSettings)
            .WithRequestValidation<UpdateGeneralTestTakingProcessSettingsRequest>()
            .AuthenticationRequired()
            .TestEditPermissionRequired();
        return group;
    }
    private async static Task<IResult> UpdateTestTakingProcessSettings(
       HttpContext httpContext,
       ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<UpdateGeneralTestTakingProcessSettingsRequest>();
        var testId = httpContext.GetTestIdFromRoute();
        var feedbackOption = request.CreateFeedbackOption().GetSuccess();
        UpdateGeneralTestTakingProcessSettingsCommand command = new(
            testId,
            request.ForceSequentialFlow,
            feedbackOption
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
        );
    }

}
