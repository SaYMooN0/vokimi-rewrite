using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity;
using TestManagingService.Domain.Common;

namespace TestManagingService.Domain.TestFeedbackRecordAggregate;

public abstract class BaseTestFeedbackRecord : AggregateRoot<TestFeedbackRecordId>
{
    protected BaseTestFeedbackRecord() { }
    public TestId TestId { get; init; }
    public AppUserId? UserId { get; init; }
    public DateTime CreatedOn { get; init; }
}