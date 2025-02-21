using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Api.Extensions;
using TestCatalogService.Application.TestComments.commands;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Api.Endpoints.view_test;

internal static class ViewTestRatingsHandlers
{
    internal static RouteGroupBuilder MapViewTestRatingsHandlers(this RouteGroupBuilder group) {
        group
            .GroupUserAccessToViewTestRequired();
        //list
        group.MapPost("/rate/{value}", RateTest)
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
    ) { }

    private static async Task<IResult> UpdateTestRating(
        HttpContext httpContext,
        ISender mediator,
        int value
    ) { }
}