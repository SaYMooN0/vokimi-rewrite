using SharedKernel.Common.domain.entity;

namespace SharedKernel.IntegrationEvents.test_managing.feedback_left;

public record BaseFeedbackForTestLeftIntegrationEvent(
    TestId TestId,
    AppUserId? AuthorId,
    DateTime CreatedOn
) : IIntegrationEvent;