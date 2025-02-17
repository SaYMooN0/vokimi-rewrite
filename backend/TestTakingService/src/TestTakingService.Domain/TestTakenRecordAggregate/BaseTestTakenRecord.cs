using SharedKernel.Common.domain;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.TestTakenRecordAggregate;

public abstract class BaseTestTakenRecord : AggregateRoot<TestTakenRecordId>
{
    protected BaseTestTakenRecord() { }
    public AppUserId? UserId { get; init; }
    public TestId TestId { get; init; }
    public abstract TestFormat TestFormat { get; }
    public DateTime TestTakingStart { get; init; }
    public DateTime TestTakingEnd { get; init; }

}