using SharedKernel.Common.domain.entity;
using TestTakingService.Domain.Common;
using TestTakingService.Domain.Common.test_taken_data.tier_list_format_test;

namespace TestTakingService.Domain.TestTakenRecordAggregate.events;

public record class TierListTestTakenEvent(
    TestTakenRecordId TestTakenRecordId,
    TestId TestId,
    AppUserId? AppUserId,
    DateTime TestTakingStart,
    DateTime TestTakingEnd,
    Dictionary<TierListTestTierId, TierListTestTakenTierData> TestTakenQuestionDetails
) : BaseTestTakenEvent(TestTakenRecordId, TestId, AppUserId, TestTakingStart, TestTakingEnd);