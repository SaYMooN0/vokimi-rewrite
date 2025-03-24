using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.interfaces;
using TestManagingService.Api.Contracts.test_feedback.general_test_format.feedback_option;
using TestManagingService.Api.Contracts.test_feedback.general_test_format.feedback_records;
using TestManagingService.Api.Extensions;
using TestManagingService.Application.TestFeedbackRecords.commands;
using TestManagingService.Application.TestFeedbackRecords.commands.general_test_format;
using TestManagingService.Application.Tests.general_format.feedback;

namespace TestManagingService.Api.Endpoints.feedback;

internal static class ManageGeneralTestFeedbackHandlers
{
    internal static RouteGroupBuilder MapManageGeneralTestFeedbackHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();

        group.MapGet("/list", ListFeedbackForGeneralTest);
        group.MapPost("/list/filtered", ListFilteredFeedbackForGeneralTest)
            .WithRequestValidation<ListFilteredGeneralTestFeedbackRecordsRequest>();

        group.MapPost("/disable", DisableFeedbackForGeneralTest);
        group.MapPost("/enable", EnableFeedbackForGeneralTest)
            .WithRequestValidation<EnableGeneralTestFeedbackOptionRequest>();

        return group;
    }

    private static async Task<IResult> ListFeedbackForGeneralTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();

        ListFeedbackForGeneralTestCommand command = new(testId);
        var feedbackRecords = await mediator.Send(command);

        return Results.Json(new { FeedbackRecords = feedbackRecords });
    }

    private static async Task<IResult> ListFilteredFeedbackForGeneralTest(
        HttpContext httpContext, ISender mediator, IDateTimeProvider dateTimeProvider
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        var request = httpContext.GetValidatedRequest<ListFilteredGeneralTestFeedbackRecordsRequest>();
        var filterResult = request.GetParsedFilter(dateTimeProvider);
        if (filterResult.IsErr(out var err)) {
            return CustomResults.ErrorResponse(err);
        }

        ListFilteredFeedbackForGeneralTestCommand command = new(testId, filterResult.GetSuccess());
        var feedbackRecords = await mediator.Send(command);

        return Results.Json(new { FeedbackRecords = feedbackRecords });

    }

    private static async Task<IResult> DisableFeedbackForGeneralTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        DisableFeedbackForGeneralTestCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result, (newFeedbackOption) => Results.Json(
            GeneralTestFeedbackOptionViewResponse.FromFeedbackOption(newFeedbackOption)
        ));
    }

    private static async Task<IResult> EnableFeedbackForGeneralTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        var request = httpContext.GetValidatedRequest<EnableGeneralTestFeedbackOptionRequest>();
        EnableFeedbackForGeneralTestCommand command = new(
            testId, request.Anonymity, request.AccompanyingText, request.MaxFeedbackLength
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result, (newFeedbackOption) => Results.Json(
            GeneralTestFeedbackOptionViewResponse.FromFeedbackOption(newFeedbackOption)
        ));
    }
}