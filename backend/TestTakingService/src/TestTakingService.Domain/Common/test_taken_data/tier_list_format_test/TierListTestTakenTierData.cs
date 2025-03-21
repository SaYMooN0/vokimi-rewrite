using SharedKernel.Common.domain.entity;

namespace TestTakingService.Domain.Common.test_taken_data.tier_list_format_test;

public record TierListTestTakenTierData(
    (TierListTestItemId item, ushort itemsOrderInTier)[] Items
);