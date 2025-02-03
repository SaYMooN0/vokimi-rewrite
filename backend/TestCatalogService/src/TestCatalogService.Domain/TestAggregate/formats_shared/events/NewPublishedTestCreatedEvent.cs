using SharedKernel.Common.domain;
using System.Collections.Immutable;

namespace TestCatalogService.Domain.TestAggregate.formats_shared.events;

public record class NewPublishedTestCreatedEvent(
    TestId TestId,
    AppUserId CreatorId,
    ImmutableHashSet<AppUserId> EditorIds
) : IDomainEvent;

