using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.tiers;

internal record class TierListTestTierInfoResponse(
    string Id,
    string Name,
    string? Description,
    ushort? MaxItemsCountLimit,
    TierListTestTierStylesContract Styles
)
{
    public static TierListTestTierInfoResponse FromTier(TierListTestTier tier) => new(
        tier.Id.ToString(),
        tier.Name,
        tier.Description,
        tier.MaxItemsCountLimit,
        TierListTestTierStylesContract.FromStyles(tier.Styles)
    );
}