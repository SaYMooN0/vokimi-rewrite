using ApiShared.extensions;
using TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.items;
using TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.tiers;
using TestCreationService.Api.Extensions;

namespace TestCreationService.Api.Endpoints.test_creation.tier_list_format;

internal static class TierListTestCreationItemOperationsHandlers
{
    internal static RouteGroupBuilder MapTierListTestCreationItemOperationsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired();

        group.MapPost("/update", UpdateTierListTestItem)
            .WithRequestValidation<UpdateTierListTestItemRequest>();
        group.MapDelete("/remove", RemoveItemFromTest);


        return group;
    }
}