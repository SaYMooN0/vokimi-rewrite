using TestTakingService.Api.Contracts.load_test_taking_data.test_formats_shared;
using TestTakingService.Domain.TestAggregate.tier_list_format;

namespace TestTakingService.Api.Contracts.load_test_taking_data.tier_list_test;

internal record class TierListTestTakingDataResponse(
    TierListTestTakingTierData[] Tiers,
    TierListTestTakingItemData[] Items,
    TestTakingStylesData Styles
)
{
    public static TierListTestTakingDataResponse FromTest(TierListFormatTest test) => new(
        test.Tiers.Select(TierListTestTakingTierData.FromTier).ToArray(),
        test.Items.Select(TierListTestTakingItemData.FromItem).ToArray(),
        TestTakingStylesData.FromStyles(test.Styles)
    );
}