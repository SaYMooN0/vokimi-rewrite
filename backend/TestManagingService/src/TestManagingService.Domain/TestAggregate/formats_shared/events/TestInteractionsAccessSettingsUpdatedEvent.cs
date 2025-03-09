using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.value_objects;

namespace TestManagingService.Domain.TestAggregate.formats_shared.events;

public record TestInteractionsAccessSettingsUpdatedEvent(
    TestId TestId,
    AccessLevel TestAccess,
    ResourceAvailabilitySetting AllowRatings,
    ResourceAvailabilitySetting AllowComments,
    bool AllowTestTakenPosts,
    bool AllowTagSuggestions
) : IDomainEvent;