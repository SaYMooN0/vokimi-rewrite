using SharedKernel.Common.domain;
using SharedKernel.Common.tests;

namespace TestTakingService.Domain.TestAggregate;

public abstract class BaseTest : AggregateRoot<TestId>
{
    protected BaseTest() { }
    public abstract TestFormat Format { get; }
    protected AppUserId _creatorId { get; init; }
}