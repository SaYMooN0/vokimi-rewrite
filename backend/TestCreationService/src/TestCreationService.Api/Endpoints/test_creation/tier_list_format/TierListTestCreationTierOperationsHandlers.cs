using ApiShared.extensions;
using TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.tiers;
using TestCreationService.Api.Extensions;

namespace TestCreationService.Api.Endpoints.test_creation.tier_list_format;

internal static class TierListTestCreationTierOperationsHandlers
{
    internal static RouteGroupBuilder MapTierListTestCreationTierOperationsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired();

        group.MapPost("/update", UpdateTierListTestTier)
            .WithRequestValidation<UpdateTierListTestTierRequest>();
        group.MapDelete("/remove", RemoveTierFromTest);


        return group;
    }
}