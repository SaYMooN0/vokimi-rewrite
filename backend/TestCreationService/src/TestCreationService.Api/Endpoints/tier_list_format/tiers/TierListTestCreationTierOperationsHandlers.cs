using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.tiers;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.Tests.tier_list_format.commands.tiers;

namespace TestCreationService.Api.Endpoints.tier_list_format.tiers;

internal static class TierListTestCreationTierOperationsHandlers
{
    internal static RouteGroupBuilder MapTierListTestCreationTierOperationsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired();

        group.MapPost("/update", UpdateTier)
            .WithRequestValidation<UpdateTierListTestTierRequest>();
        group.MapDelete("/remove", RemoveTierFromTest);


        return group;
    }

    private static async Task<IResult> UpdateTier(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        TierListTestTierId itemId = httpContext.GetTierIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateTierListTestTierRequest>();

        UpdateTierListTestTierCommand command = new(
            testId, itemId, request.Name, request.Description, request.MaxItemsCountLimit,
            request.Styles.ParseToTierStyles().GetSuccess()
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result, (tier) => Results.Json(new {
                TierListTestItems = TierListTestTierInfoResponse.FromTier(tier)
            })
        );
    }

    private static async Task<IResult> RemoveTierFromTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        TierListTestTierId tierId = httpContext.GetTierIdFromRoute();

        RemoveTierListTestTierCommand command = new(testId, tierId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(result, () => Results.Ok());
    }
}