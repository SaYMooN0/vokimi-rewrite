using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.items;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.Tests.tier_list_format.commands.items;

namespace TestCreationService.Api.Endpoints.tier_list_format.items;

internal static class TierListTestCreationItemsHandlers
{
    internal static RouteGroupBuilder MapTierListTestCreationItemsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired();

        group.MapPost("/saveNew", SaveNewItem)
            .WithRequestValidation<SaveItemForTierListTestRequest>();
        group.MapPost("/updateOrder", UpdateTestItemsOrder)
            .WithRequestValidation<UpdateTierListTestItemsOrderRequest>();

        return group;
    }

    private static async Task<IResult> SaveNewItem(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        var request = httpContext.GetValidatedRequest<SaveItemForTierListTestRequest>();

        SaveNewItemForTierListTestCommand command = new(
            testId,
            request.ItemName,
            request.ItemClarification,
            request.ParsedItemContentData().GetSuccess()
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result, (newItem) => Results.Json(new {
                TierListTestItems = TierListTestItemInfoResponse.FromItem(newItem)
            })
        );
    }

    private static async Task<IResult> UpdateTestItemsOrder(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateTierListTestItemsOrderRequest>();
        var orderController = request.CreateOrderController().GetSuccess();

        UpdateTierListTestItemsOrderCommand command = new(testId, orderController);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(result, () => Results.Ok());
    }
}