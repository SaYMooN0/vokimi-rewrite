using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.tiers;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.Tests.tier_list_format.commands.tiers;

namespace TestCreationService.Api.Endpoints.tier_list_format.tiers;

internal static class TierListTestCreationTiersHandlers
{
    internal static RouteGroupBuilder MapTierListTestCreationTiersHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired();

        group.MapPost("/createNew", AddNewTier)
            .WithRequestValidation<AddNewTierListTestTierRequest>();
        group.MapPost("/updateOrder", UpdateTiersOrder)
            .WithRequestValidation<UpdateTierListTestTiersOrderRequest>();


        return group;
    }

    private static async Task<IResult> AddNewTier(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        var request = httpContext.GetValidatedRequest<AddNewTierListTestTierRequest>();

        AddNewTierListTestTierCommand command = new(testId, request.TierName);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result, (newTier) => Results.Json(new {
                TierListTestItems = TierListTestTierInfoResponse.FromTier(newTier)
            })
        );
    }

    private static async Task<IResult> UpdateTiersOrder(
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