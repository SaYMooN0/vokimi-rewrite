using SharedKernel.Common.domain.entity;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.TestTakenRecordAggregate.events;

public record class TierListTestTakenEvent(
    TestTakenRecordId TestTakenRecordId,
    TestId TestId,
    AppUserId? AppUserId,
    DateTime TestTakingStart,
    DateTime TestTakingEnd
) : BaseTestTakenEvent(TestTakenRecordId, TestId, AppUserId, TestTakingStart, TestTakingEnd);