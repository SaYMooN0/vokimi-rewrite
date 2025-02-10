using SharedKernel.Common.domain;
using SharedKernel.Common.interfaces;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.TestTakenRecordAggregate;

public abstract class BaseTestTakenRecord : AggregateRoot<TestTakenRecordId>
{
    protected BaseTestTakenRecord() { }
    public AppUserId UserId { get; init; }
    public TestId TestId { get; init; }
    public DateTime TestTakingStart { get; init; }
    public DateTime TestTakingEnd { get; init; }

}