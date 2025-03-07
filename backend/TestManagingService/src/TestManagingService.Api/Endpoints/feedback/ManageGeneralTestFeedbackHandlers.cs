using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestManagingService.Api.Contracts.test_feedback;
using TestManagingService.Api.Extensions;
using TestManagingService.Application.Tests.general_format.feedback;

namespace TestManagingService.Api.Endpoints.feedback;

internal static class ManageGeneralTestFeedbackHandlers
{
    internal static RouteGroupBuilder MapManageGeneralTestFeedbackHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();

        group.MapPost("/disable",DisableFeedbackForGeneralTest);
        group.MapPost("/enable", EnableFeedbackForGeneralTest)
            .WithRequestValidation<EnableGeneralTestFeedbackRequest>();

        //list left feedback + filter
        return group;
    }

    private static async Task<IResult> DisableFeedbackForGeneralTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        DisableFeedbackForGeneralTestCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result, (newFeedbackOption) => Results.Json(
            GeneralTestFeedbackViewResponse.FromFeedbackOption(newFeedbackOption)
        ));
    }
    private static async Task<IResult> EnableFeedbackForGeneralTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        var request = httpContext.GetValidatedRequest<EnableGeneralTestFeedbackRequest>();
        EnableFeedbackForGeneralTestCommand command = new(
            testId, request.Anonymity, request.AccompanyingText, request.MaxFeedbackLength
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result, (newFeedbackOption) => Results.Json(
            GeneralTestFeedbackViewResponse.FromFeedbackOption(newFeedbackOption)
        ));
    }
}