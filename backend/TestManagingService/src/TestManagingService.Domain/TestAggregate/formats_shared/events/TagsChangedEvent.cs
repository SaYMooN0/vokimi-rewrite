using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;

namespace TestManagingService.Domain.TestAggregate.formats_shared.events;

public record TagsChangedEvent(
    TestId TestId,
    HashSet<TestTagId> NewTags
) : IDomainEvent;