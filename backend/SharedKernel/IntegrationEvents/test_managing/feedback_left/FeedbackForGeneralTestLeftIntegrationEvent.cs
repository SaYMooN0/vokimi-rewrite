using SharedKernel.Common.domain.entity;

namespace SharedKernel.IntegrationEvents.test_managing.feedback_left;

public record FeedbackForGeneralTestLeftIntegrationEvent(
    TestId TestId,
    AppUserId? AuthorId,
    DateTime CreatedOn,
    string Text,
    bool WasLeftAnonymously
) : BaseFeedbackForTestLeftIntegrationEvent(TestId, AuthorId, CreatedOn);