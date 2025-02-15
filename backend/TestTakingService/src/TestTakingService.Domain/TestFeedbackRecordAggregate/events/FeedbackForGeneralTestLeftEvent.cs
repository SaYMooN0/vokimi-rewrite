using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.TestFeedbackRecordAggregate.events;

public record FeedbackForGeneralTestLeftEvent(
    TestFeedbackRecordId FeedbackRecordId,
    TestId TestId,
    AppUserId? AppUserId,
    TestTakenRecordId TestTakenRecordId,
    DateTime CreatedOn,
    string Text,
    bool WasLeftAnonymously
) : BaseFeedbackForTestLeftEvent(FeedbackRecordId, TestId, AppUserId, TestTakenRecordId, CreatedOn);