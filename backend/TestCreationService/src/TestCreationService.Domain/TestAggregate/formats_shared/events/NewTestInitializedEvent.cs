using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;

namespace TestCreationService.Domain.TestAggregate.formats_shared.events;

public record class NewTestInitializedEvent(TestId TestId, AppUserId CreatorId) : IDomainEvent;