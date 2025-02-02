using SharedKernel.Common;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Domain.TestTagAggregate;

internal class TestTag : AggregateRoot<TestTagId>
{
    //id is the tag value
    private TestTag() { }
    public int TestWithThisTagCount { get; private set; }
    public static ErrOr<TestTag> CreateNew(string value) {
        if (TestTagsRules.IsStringValidTag(value)) {
            return new TestTag() {
                Id = new TestTagId(value),
                TestWithThisTagCount = 0
            };
        }
        return Err.ErrFactory.InvalidData($"'{value}' is not a valid tag");
    }
    public override string ToString() => Id.ToString();
    public void IncrementTestWithThisTagCount() => TestWithThisTagCount++;
    public void DecrementTestWithThisTagCount() => TestWithThisTagCount--;
}
