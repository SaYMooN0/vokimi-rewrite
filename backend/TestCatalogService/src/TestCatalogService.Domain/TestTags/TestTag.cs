using SharedKernel.Common;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Domain.TestTags;

internal class TestTag : AggregateRoot<TestTagId>
{
    public string Value { get; init; }
    //list of tests
    public override string ToString() {
        return Value;
    }
}
