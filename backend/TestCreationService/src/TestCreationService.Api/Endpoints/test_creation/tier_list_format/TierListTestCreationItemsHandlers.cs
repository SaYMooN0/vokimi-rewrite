using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.items;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.Tests.tier_list_format.commands.items;

namespace TestCreationService.Api.Endpoints.test_creation.tier_list_format;

internal static class TierListTestCreationItemsHandlers
{
    internal static RouteGroupBuilder MapTierListTestCreationItemsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired();

        group.MapPost("/saveNew", SaveNewItemForTierListTest)
            .WithRequestValidation<SaveNewItemForTierListTestRequest>();
        group.MapPost("/updateOrder", UpdateTestItemsOrder)
            .WithRequestValidation<UpdateTierListTestItemsOrderRequest>();

        return group;
    }

    private static async Task<IResult> SaveNewItemForTierListTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        var request = httpContext.GetValidatedRequest<SaveNewItemForTierListTestRequest>();

        SaveNewItemForTierListTestCommand command = new(
            testId,
            request.ItemName,
            request.ItemClarification,
            request.ParsedItemContentData().GetSuccess()
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            () => Results.Ok()
        );
    }

    private static async Task<IResult> UpdateTestItemsOrder(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateTierListTestItemsOrderRequest>();
    }
}