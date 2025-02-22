using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Api.Extensions;
using TestCatalogService.Application.Tests.formats_shared.commands.ratings;

namespace TestCatalogService.Api.Endpoints.view_test;

internal static class ViewTestRatingsHandlers
{
    internal static RouteGroupBuilder MapViewTestRatingsHandlers(this RouteGroupBuilder group) {
        group
            .GroupUserAccessToViewTestRequired();
        //list
        group.MapPost("/add/{value}", RateTest)
            .AuthenticationRequired()
            .WithAccessCheckToRateTest();
        group.MapPost("/updateRating/{value}", UpdateTestRating)
            .AuthenticationRequired()
            .WithAccessCheckToRateTest();

        return group;
    }

    private static async Task<IResult> RateTest(
        HttpContext httpContext,
        ISender mediator,
        int value
    ) {
        AppUserId userId = httpContext.GetAuthenticatedUserId();
        TestId testId = httpContext.GetTestIdFromRoute();

        RateTestCommand command = new(userId, testId, value);
        var result = await mediator.Send(command);
        return CustomResults.FromErrOr(result,
            (ratingValue) => Results.Json(new { RatingValue = ratingValue })
        );
    }

    private static async Task<IResult> UpdateTestRating(
        HttpContext httpContext,
        ISender mediator,
        int value
    ) {
        AppUserId userId = httpContext.GetAuthenticatedUserId();
        TestId testId = httpContext.GetTestIdFromRoute();

        UpdateTestRatingCommand command = new(userId, testId, value);
        var result = await mediator.Send(command);
        return CustomResults.FromErrOr(result,
            (ratingValue) => Results.Json(new { RatingValue = ratingValue })
        );
    }
}