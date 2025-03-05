using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.value_objects;

namespace SharedKernel.IntegrationEvents.test_managing;

public record TestInteractionsAccessSettingsUpdatedIntegrationEvent(
    TestId TestId,
    AccessLevel TestAccess,
    ResourceAvailabilitySetting AllowRatings,
    ResourceAvailabilitySetting AllowComments,
    bool AllowTestTakenPosts,
    bool AllowTagSuggestions
) : IIntegrationEvent;