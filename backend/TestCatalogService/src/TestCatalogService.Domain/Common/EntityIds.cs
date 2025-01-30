using SharedKernel.Common.EntityIds;

namespace TestCatalogService.Domain.Common;

public class TestTagId : EntityId
{
    public TestTagId(Guid value) : base(value) { }
    public static TestTagId CreateNew() => new(Guid.CreateVersion7());
}