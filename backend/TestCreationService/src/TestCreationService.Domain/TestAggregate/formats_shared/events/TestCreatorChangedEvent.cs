using SharedKernel.Common;
using SharedKernel.Common.EntityIds;

namespace TestCreationService.Domain.TestAggregate.formats_shared.events;

public record class TestCreatorChangedEvent(
    TestId TestId,
    AppUserId OldCreator,
    AppUserId NewCreator
) : IDomainEvent;
