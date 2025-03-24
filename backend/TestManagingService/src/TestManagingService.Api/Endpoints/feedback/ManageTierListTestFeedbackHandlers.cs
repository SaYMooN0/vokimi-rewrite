using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.interfaces;
using TestManagingService.Api.Contracts.test_feedback.tier_list_test_format.feedback_option;
using TestManagingService.Api.Contracts.test_feedback.tier_list_test_format.feedback_records;
using TestManagingService.Api.Extensions;
using TestManagingService.Application.TestFeedbackRecords.commands.tier_list_test_format;
using TestManagingService.Application.Tests.tier_list_format.feedback;

namespace TestManagingService.Api.Endpoints.feedback;

internal static class ManageTierListTestFeedbackHandlers
{
    internal static RouteGroupBuilder MapManageTierListTestFeedbackHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();

        group.MapGet("/list", ListFeedbackForTierListTest);
        group.MapPost("/list/filtered", ListFilteredFeedbackForTierListTest)
            .WithRequestValidation<ListFilteredTierListTestFeedbackRecordsRequest>();

        group.MapPost("/disable", DisableFeedbackForTierListTest);
        group.MapPost("/enable", EnableFeedbackForTierListTest)
            .WithRequestValidation<EnableTierListTestFeedbackOptionRequest>();

        return group;
    }
    private static async Task<IResult> ListFeedbackForTierListTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();

        ListFeedbackForTierListTestCommand command = new(testId);
        var feedbackRecords = await mediator.Send(command);

        return Results.Json(new { FeedbackRecords = feedbackRecords });
    }

    private static async Task<IResult> ListFilteredFeedbackForTierListTest(
        HttpContext httpContext, ISender mediator, IDateTimeProvider dateTimeProvider
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        var request = httpContext.GetValidatedRequest<ListFilteredTierListTestFeedbackRecordsRequest>();
        var filterResult = request.GetParsedFilter(dateTimeProvider);
        if (filterResult.IsErr(out var err)) {
            return CustomResults.ErrorResponse(err);
        }

        ListFilteredFeedbackForTierListTestCommand command = new(testId, filterResult.GetSuccess());
        var feedbackRecords = await mediator.Send(command);

        return Results.Json(new { FeedbackRecords = feedbackRecords });

    }

    private static async Task<IResult> DisableFeedbackForTierListTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        DisableFeedbackForTierListTestCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result, (newFeedbackOption) => Results.Json(
            TierListTestFeedbackOptionViewResponse.FromFeedbackOption(newFeedbackOption)
        ));
    }

    private static async Task<IResult> EnableFeedbackForTierListTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        var request = httpContext.GetValidatedRequest<EnableTierListTestFeedbackOptionRequest>();
        EnableFeedbackForTierListTestCommand command = new(
            testId, request.Anonymity, request.AccompanyingText, request.MaxFeedbackLength
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result, (newFeedbackOption) => Results.Json(
            TierListTestFeedbackOptionViewResponse.FromFeedbackOption(newFeedbackOption)
        ));
    }
}