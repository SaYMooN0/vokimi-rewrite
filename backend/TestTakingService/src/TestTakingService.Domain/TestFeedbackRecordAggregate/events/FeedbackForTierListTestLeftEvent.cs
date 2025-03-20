using SharedKernel.Common.domain.entity;

namespace TestTakingService.Domain.TestFeedbackRecordAggregate.events;

public record class FeedbackForTierListTestLeftEvent(
    TestId TestId,
    AppUserId? AuthorId,
    DateTime CreatedOn,
    string Text,
    bool WasLeftAnonymously
) : BaseFeedbackForTestLeftEvent(TestId, AuthorId, CreatedOn);