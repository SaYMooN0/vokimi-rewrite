using SharedKernel.Common.domain;
using SharedKernel.Common.domain.interfaces;

namespace TestCatalogService.Domain.Common;

public class TestTagId : IEntityId
{
    public string Value { get; init; }
    public TestTagId(string value) { Value = value; }
    public override string ToString() => Value;
}