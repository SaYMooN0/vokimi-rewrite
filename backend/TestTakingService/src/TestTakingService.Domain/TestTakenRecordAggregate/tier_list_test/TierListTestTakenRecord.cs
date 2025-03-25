using System.Collections.Immutable;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.TestTakenRecordAggregate.tier_list_test;

public class TierListTestTakenRecord : BaseTestTakenRecord
{
    private TierListTestTakenRecord() { }
    public override TestFormat TestFormat => TestFormat.TierList;
    public IReadOnlyCollection<TierListTestTakenRecordTierDetails> TierDetails { get; private init; }


    public TierListTestTakenRecord(
        TestTakenRecordId id,
        AppUserId? userId,
        TestId testId,
        DateTime testTakingStart,
        DateTime testTakingEnd,
        //general test taken record specific
        ImmutableArray<TierListTestTakenRecordTierDetails> tierDetails
    ) {
        Id = id;
        UserId = userId;
        TestId = testId;
        TestTakingStart = testTakingStart;
        TestTakingEnd = testTakingEnd;
        TierDetails = tierDetails;
    }
}