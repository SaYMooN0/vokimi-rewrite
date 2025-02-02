using SharedKernel.Common.domain;

namespace TestCreationService.Domain.TestAggregate.formats_shared.events;

public record class TestEditorsListChangedEvent(
    TestId TestId,
    ISet<AppUserId> newEditors,
    IEnumerable<AppUserId> oldEditors
) : IDomainEvent;

