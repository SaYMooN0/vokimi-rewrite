using SharedKernel.Common.domain;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.TestFeedbackRecordAggregate;

public abstract class BaseTestFeedbackRecord : AggregateRoot<TestFeedbackRecordId>
{
    protected BaseTestFeedbackRecord() { }
    public TestId TestId { get; init; }
    public AppUserId? UserId { get; init; }
    public TestTakenRecordId TestTakenRecordId { get; init; }
    public DateTime CreatedOn { get; init; }
}