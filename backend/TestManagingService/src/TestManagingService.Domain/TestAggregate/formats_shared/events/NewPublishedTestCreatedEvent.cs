using System.Collections.Immutable;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;

namespace TestManagingService.Domain.TestAggregate.formats_shared.events;

public record class NewPublishedTestCreatedEvent(
    TestId TestId,
    AppUserId CreatorId,
    ImmutableHashSet<AppUserId> EditorIds
) : IDomainEvent;

        