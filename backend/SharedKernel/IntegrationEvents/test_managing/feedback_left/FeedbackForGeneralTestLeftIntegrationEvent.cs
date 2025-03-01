using SharedKernel.Common.domain.entity;

namespace SharedKernel.IntegrationEvents.test_managing.feedback_left;

public record FeedbackForGeneralTestLeftIntegrationEvent(
    TestId TestId,
    AppUserId? AppUserId,
    DateTime CreatedOn,
    string Text,
    bool WasLeftAnonymously
) : BaseFeedbackForTestLeftIntegrationEvent(TestId, AppUserId, CreatedOn);