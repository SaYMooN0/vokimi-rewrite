using SharedKernel.Common.domain.entity;

namespace SharedKernel.Common.tests.tier_list_format;

public class TierListTestTierStyles : Entity<TierListTestTierStylesId>
{
    private TierListTestTierStyles() { }
    public static TierListTestTierStyles Default() => new();
}