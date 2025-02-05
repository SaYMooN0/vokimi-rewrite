using SharedKernel.Common.domain;
using SharedKernel.Common.tests;

namespace TestTakingService.Domain.TestAggregate;

public abstract class BaseTest : AggregateRoot<TestId>
{
    protected BaseTest() { }
    public abstract TestFormat Format { get; }
    public AppUserId CreatorId { get; init; }
}
