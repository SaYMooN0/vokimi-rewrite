using ApiShared;
using ApiShared.extensions;
using MediatR;
using TestCreationService.Api.Extensions;

namespace TestCreationService.Api.Endpoints.test_creation.tier_list_format;


internal static class TierListFormatTestCreationHandlers
{
    internal static RouteGroupBuilder MapTierListFormatTestCreationHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired();

        // group
        //     .MapPost("/updateFeedbackOption", UpdateTestFeedbackOption)
        //     .WithRequestValidation<UpdateTierListTestFeedbackCommandRequest>();
        return group;
    }
    // private static async Task<IResult> UpdateTestFeedbackOption(
    //     HttpContext httpContext,
    //     ISender mediator
    // ) {
    //     var request = httpContext.GetValidatedRequest<UpdateTierListTestFeedbackCommandRequest>();
    //     var testId = httpContext.GetTestIdFromRoute();
    //     var feedbackOption = request.CreateFeedbackOption().GetSuccess();
    //
    //     UpdateTierListTestFeedbackCommand command = new(testId, feedbackOption);
    //     var result = await mediator.Send(command);
    //
    //     return CustomResults.FromErrOrNothing(
    //         result,
    //         () => Results.Ok()
    //     );
    // }
}