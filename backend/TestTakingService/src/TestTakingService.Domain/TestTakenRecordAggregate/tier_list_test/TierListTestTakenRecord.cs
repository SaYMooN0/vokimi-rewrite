using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.TestTakenRecordAggregate.tier_list_test;

public class TierListTestTakenRecord : BaseTestTakenRecord
{
    private TierListTestTakenRecord() { }
    public override TestFormat TestFormat => TestFormat.TierList;

    public static TierListTestTakenRecord CreateNew(
        AppUserId? userId,
        TestId testId,
        DateTime testTakingStart,
        DateTime testTakingEnd
    ) {
        TierListTestTakenRecord record = new() {
            Id = TestTakenRecordId.CreateNew(),
            UserId = userId,
            TestId = testId,
            TestTakingStart = testTakingStart,
            TestTakingEnd = testTakingEnd
        };

        return record;
    }
}