using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;

namespace TestCreationService.Domain.TestAggregate.general_format.events;

public record GeneralFormatTestDeletedEvent(
    TestId TestId,
    AppUserId CreatorId,
    HashSet<AppUserId> EditorIds
) : IDomainEvent;