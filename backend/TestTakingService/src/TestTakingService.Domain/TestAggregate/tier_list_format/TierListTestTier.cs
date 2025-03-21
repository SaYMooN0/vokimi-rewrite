using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.tier_list_format;

namespace TestTakingService.Domain.TestAggregate.tier_list_format;

public class TierListTestTier : Entity<TierListTestTierId>
{
    private TierListTestTier() { }
    public string Name { get; }
    public string? Description { get; }
    public ushort? MaxItemsCountLimit { get; } //if null => disabled
    public TierListTestTierStyles Styles { get; }
    public ushort OrderInTest { get; }

    public TierListTestTier(
        TierListTestTierId id,
        string name,
        string? description,
        ushort? maxItemsCountLimit,
        TierListTestTierStyles styles,
        ushort orderInTest
    ) {
        Id = id;
        Name = name;
        Description = description;
        MaxItemsCountLimit = maxItemsCountLimit;
        Styles = styles;
        OrderInTest = orderInTest;
    }
}