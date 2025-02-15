using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;

namespace TestCreationService.Domain.TestAggregate.formats_shared.events;

public record class TestCreatorChangedEvent(
    TestId TestId,
    AppUserId OldCreator,
    AppUserId NewCreator
) : IDomainEvent;
