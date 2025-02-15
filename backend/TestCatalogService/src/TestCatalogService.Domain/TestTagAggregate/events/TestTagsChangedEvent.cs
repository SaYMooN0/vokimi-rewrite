using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Domain.TestTagAggregate.events;
public record class TestTagsChangedEvent(
    TestId TestId,
    ISet<TestTagId> OldTags,
    ISet<TestTagId> NewTags
) : IDomainEvent;
