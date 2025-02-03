using SharedKernel.Common;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.interfaces;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.TestTagAggregate;

namespace TestCatalogService.Domain.Common;

public class TestTagId : IEntityId
{
    public string Value { get; init; }
    public TestTagId(string value) {
        if (!TestTagsRules.IsStringValidTag(value)) {
            throw new ErrCausedException(Err.ErrFactory.InvalidData($"'{value}' is not a valid tag"));
        }
        Value = value;
    }
    public static ErrOr<TestTagId> Create(string value) {
        if (!TestTagsRules.IsStringValidTag(value)) {
            return Err.ErrFactory.InvalidData($"'{value}' is not a valid tag");
        }
        return new TestTagId(value);
    }
    public override string ToString() => Value;
}