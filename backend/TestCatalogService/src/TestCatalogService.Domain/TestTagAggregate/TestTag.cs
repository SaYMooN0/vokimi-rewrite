using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity;

namespace TestCatalogService.Domain.TestTagAggregate;

public class TestTag : AggregateRoot<TestTagId>
{
    //id is the tag value
    private TestTag() { }
    public int TestWithThisTagCount { get; private set; }
    public static TestTag Create(TestTagId testTagId) => new() {
        Id = testTagId,
        TestWithThisTagCount = 0
    };
    public override string ToString() => Id.ToString();
    public void IncrementTestWithThisTagCount() => TestWithThisTagCount++;
    public void DecrementTestWithThisTagCount() => TestWithThisTagCount--;
}
