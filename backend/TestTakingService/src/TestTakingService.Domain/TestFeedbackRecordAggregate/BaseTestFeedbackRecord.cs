using SharedKernel.Common.domain;
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