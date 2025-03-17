using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.tiers;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.Tests.tier_list_format.commands.tiers;

namespace TestCreationService.Api.Endpoints.test_creation.tier_list_format;

internal static class TierListTestCreationTiersHandlers
{
    internal static RouteGroupBuilder MapTierListTestCreationTiersHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired();

        group.MapPost("/createNew", CreateNewTier);
        group.MapPost("/updateOrder", UpdateItemsOrder)
            .WithRequestValidation<UpdateTierListTestTiersOrderRequest>();


        return group;
    }
    private static async Task<IResult> CreateNewTier(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();

         command = new(
            testId,
            request.ItemName,
            request.ItemClarification,
            request.ParsedItemContentData().GetSuccess()
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result, (newTier) => Results.Json(new {
                TierListTestItems = TierListTestTierInfoResponse.FromTier(newTier)
            })
        );
    }

    private static async Task<IResult> UpdateTestItemsOrder(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateTierListTestTiersOrderRequest>();
        var orderController = request.CreateOrderController().GetSuccess();

        UpdateTierListTestTiersOrderCommand command = new(testId, orderController);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(result, () => Results.Ok());
    }
}