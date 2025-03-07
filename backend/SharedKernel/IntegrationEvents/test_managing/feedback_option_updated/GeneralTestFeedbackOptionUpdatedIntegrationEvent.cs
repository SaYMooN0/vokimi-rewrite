using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.general_format;

namespace SharedKernel.IntegrationEvents.test_managing.feedback_option_updated;

public record GeneralTestFeedbackOptionUpdatedIntegrationEvent(
    TestId TestId,
    GeneralTestFeedbackOption NewFeedbackOption
) : IIntegrationEvent;