using SharedKernel.Common.EntityIds;
using SharedKernel.Common;

namespace TestCreationService.Domain.TestAggregate.formats_shared.events;

public record class NewTestInitializedEvent(TestId TestId, AppUserId CreatorId) : IDomainEvent;