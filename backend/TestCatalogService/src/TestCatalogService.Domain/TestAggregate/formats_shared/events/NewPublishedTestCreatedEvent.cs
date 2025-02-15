using SharedKernel.Common.domain;
using System.Collections.Immutable;
using SharedKernel.Common.domain.entity_id;

namespace TestCatalogService.Domain.TestAggregate.formats_shared.events;

public record class NewPublishedTestCreatedEvent(
    TestId TestId,
    AppUserId CreatorId,
    ImmutableHashSet<AppUserId> EditorIds
) : IDomainEvent;

