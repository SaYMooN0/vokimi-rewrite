using SharedKernel.Common.domain;

namespace TestCreationService.Domain.TestAggregate.formats_shared.events;

public record class TestCreatorChangedEvent(
    TestId TestId,
    AppUserId OldCreator,
    AppUserId NewCreator
) : IDomainEvent;
