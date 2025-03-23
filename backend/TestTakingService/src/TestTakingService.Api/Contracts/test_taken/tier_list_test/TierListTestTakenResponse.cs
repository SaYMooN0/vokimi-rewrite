using SharedKernel.Common.domain.entity;
using TestTakingService.Domain.Common.test_taken_data.tier_list_format_test;

namespace TestTakingService.Api.Contracts.test_taken.tier_list_test;

public record class TierListTestTakenResponse(
    Dictionary<TierListTestTierId, TierListTestTakenTierData> FinalTierList
);