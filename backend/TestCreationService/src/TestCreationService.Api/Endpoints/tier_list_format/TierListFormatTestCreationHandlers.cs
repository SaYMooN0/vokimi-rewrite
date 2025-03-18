using ApiShared;
using ApiShared.extensions;
using MediatR;
using TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.feedback_option;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.Tests.tier_list_format.commands;

namespace TestCreationService.Api.Endpoints.tier_list_format;

internal static class TierListFormatTestCreationHandlers
{
    internal static RouteGroupBuilder MapTierListFormatTestCreationHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired();

        group.MapGet("/feedbackOptionInfo", GetTestFeedbackOption);
        group
            .MapPost("/updateFeedbackOption", UpdateTestFeedbackOption)
            .WithRequestValidation<UpdateTierListTestFeedbackOptionRequest>();
        return group;
    }

    private static async Task<IResult> GetTestFeedbackOption(
        HttpContext httpContext, ISender mediator
    ) {
        var testId = httpContext.GetTestIdFromRoute();

        GetTierListTestFeedbackOptionCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result, (option) => Results.Json(new {
                feedback = TierListTestFeedbackOptionInfoResponse.FromFeedbackOption(option)
            })
        );
    }

    private static async Task<IResult> UpdateTestFeedbackOption(
        HttpContext httpContext, ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<UpdateTierListTestFeedbackOptionRequest>();
        var testId = httpContext.GetTestIdFromRoute();
        var feedbackOption = request.CreateFeedbackOption().GetSuccess();

        UpdateTierListTestFeedbackCommand command = new(testId, feedbackOption);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result, (option) => Results.Json(new {
                feedback = TierListTestFeedbackOptionInfoResponse.FromFeedbackOption(option)
            })
        );
    }
}