using TestTakingService.Domain.TestAggregate.tier_list_format;

namespace TestTakingService.Api.Contracts.load_test_taking_data.tier_list_test;

internal record class TierListTestTakingTierData(
    string Id,
    ushort Order,
    string Name,
    string? Description,
    ushort? MaxItemsCountLimit,
    TierListTestTakingTierStylesData Styles
)
{
    public static TierListTestTakingTierData FromTier(TierListTestTier tier) => new(
        tier.Id.ToString(),
        tier.OrderInTest,
        tier.Name,
        tier.Description,
        tier.MaxItemsCountLimit,
        new TierListTestTakingTierStylesData(
            tier.Styles.BackgroundColor.ToString(),
            tier.Styles.TextColor.ToString()
        )
    );
}

internal record class TierListTestTakingTierStylesData(string BackGroundColor, string TextColor);