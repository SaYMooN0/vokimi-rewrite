using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.tier_list_format.feedback;

namespace SharedKernel.IntegrationEvents.test_managing.feedback_option_updated;

public record TierListTestFeedbackOptionUpdatedIntegrationEvent(
    TestId TestId,
    TierListTestFeedbackOption NewFeedbackOption
) : IIntegrationEvent;