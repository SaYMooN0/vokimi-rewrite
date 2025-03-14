using ApiShared.extensions;
using TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.items;
using TestCreationService.Api.Extensions;

namespace TestCreationService.Api.Endpoints.test_creation.tier_list_format;

internal static class TierListTestCreationItemsHandlers
{
    internal static RouteGroupBuilder MapTierListTestCreationItemsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired();

        group.MapPost("/saveNew", SaveNewTierListTestItem);
        group.MapPost("/updateOrder", UpdateItemsOrder)
            .WithRequestValidation<UpdateTierListTestItemsOrderRequest>();



        return group;
    }
}