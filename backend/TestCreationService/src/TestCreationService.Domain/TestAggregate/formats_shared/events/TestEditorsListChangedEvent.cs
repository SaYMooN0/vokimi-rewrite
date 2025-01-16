using SharedKernel.Common;
using SharedKernel.Common.EntityIds;

namespace TestCreationService.Domain.TestAggregate.formats_shared.events;

public record class TestEditorsListChangedEvent(
    TestId TestId,
    HashSet<AppUserId> newEditors,
    IEnumerable<AppUserId> oldEditors
) : IDomainEvent;

