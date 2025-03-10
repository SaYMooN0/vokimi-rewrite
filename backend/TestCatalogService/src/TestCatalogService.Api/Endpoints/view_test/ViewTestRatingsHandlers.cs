using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.interfaces;
using SharedKernel.Configs;
using TestCatalogService.Api.Contracts.view_test.comments.view_response;
using TestCatalogService.Api.Contracts.view_test.ratings;
using TestCatalogService.Api.Extensions;
using TestCatalogService.Application.Tests.formats_shared.commands.ratings;

namespace TestCatalogService.Api.Endpoints.view_test;

internal static class ViewTestRatingsHandlers
{
    internal static RouteGroupBuilder MapViewTestRatingsHandlers(this RouteGroupBuilder group) {
        group
            .GroupUserAccessToViewTestRequired();

        group.MapGet("/list/{package}", ListTestRatings);
        group.MapPost("/listFiltered/{package}", ListTestFilteredRatings)
            .WithRequestValidation<ListFilteredTestRatingsRequest>();
        group.MapPost("/add/{value}", RateTest)
            .AuthenticationRequired();
        group.MapPost("/updateRating/{value}", UpdateTestRating)
            .AuthenticationRequired();


        return group;
    }

    private static async Task<IResult> ListTestRatings(
        HttpContext httpContext,
        ISender mediator,
        int package
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();

        ListTestRatingsCommand command = new(testId, package);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result,
            (ratings) => Results.Json(
                ViewTestRatingsListResponse.FromTestRatings(ratings)
            )
        );
    }

    private static async Task<IResult> ListTestFilteredRatings(
        HttpContext httpContext,
        ISender mediator,
        JwtTokenConfig jwtTokenConfig,
        IDateTimeProvider dateTimeProvider,
        int package
    ) {
        var req = httpContext.GetValidatedRequest<ListFilteredTestRatingsRequest>();
        TestId testId = httpContext.GetTestIdFromRoute();
        AppUserId? viewerId = null;
        httpContext.ParseUserIdFromJwtToken(jwtTokenConfig).IsSuccess(out viewerId);
        var filter = req.ParseToFilter(dateTimeProvider, viewerId);
        if (filter.IsErr(out var filterErr)) {
            return CustomResults.ErrorResponse(filterErr);
        }

        ListFilteredTestRatingsCommand command = new(testId, viewerId, filter.GetSuccess(), package);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result,
            (ratings) => Results.Json(
                ViewTestRatingsListResponse.FromTestRatings(ratings)
            )
        );
    }

    private static async Task<IResult> RateTest(
        HttpContext httpContext, ISender mediator, int value
    ) {
        AppUserId userId = httpContext.GetAuthenticatedUserId();
        TestId testId = httpContext.GetTestIdFromRoute();

        AddTestRatingCommand ratingCommand = new(userId, testId, value);
        var result = await mediator.Send(ratingCommand);

        return CustomResults.FromErrOr(result,
            (rating) => Results.Json(new { Rating = TestRatingDataViewResponse.FromTestRating(rating) })
        );
    }

    private static async Task<IResult> UpdateTestRating(
        HttpContext httpContext, ISender mediator, int value
    ) {
        AppUserId userId = httpContext.GetAuthenticatedUserId();
        TestId testId = httpContext.GetTestIdFromRoute();

        UpdateTestRatingCommand command = new(userId, testId, value);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result,
            (rating) => Results.Json(new { Rating = TestRatingDataViewResponse.FromTestRating(rating) })
        );
    }
}