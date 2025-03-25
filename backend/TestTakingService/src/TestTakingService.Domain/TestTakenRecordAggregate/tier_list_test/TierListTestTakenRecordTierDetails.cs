using SharedKernel.Common.domain.entity;
using TestTakingService.Domain.Common;
using TestTakingService.Domain.Common.test_taken_data.tier_list_format_test;

namespace TestTakingService.Domain.TestTakenRecordAggregate.tier_list_test;

public class TierListTestTakenRecordTierDetails : Entity<TierListTestTakenRecordDetailsId>
{
    private TierListTestTakenRecordTierDetails() { }
    public TierListTestTierId TierId { get; init; }
    public TierListTestTakenTierData ItemsWithOrder { get; init; }


    public static TierListTestTakenRecordTierDetails CreateNew(
        TierListTestTierId tierId,
        TierListTestTakenTierData items
    ) => new() {
        Id = TierListTestTakenRecordDetailsId.CreateNew(),
        TierId = tierId,
        ItemsWithOrder = items,
    };
}