using SharedKernel.Common.domain;

namespace TestCreationService.Domain.TestAggregate.formats_shared.events;

public record class NewTestInitializedEvent(TestId TestId, AppUserId CreatorId) : IDomainEvent;