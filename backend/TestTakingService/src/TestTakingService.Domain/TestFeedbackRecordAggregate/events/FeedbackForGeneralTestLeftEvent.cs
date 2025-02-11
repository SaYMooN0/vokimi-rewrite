using SharedKernel.Common.domain;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.TestFeedbackRecordAggregate.events;

public record FeedbackForGeneralTestLeftEvent(
    TestId TestId,
    AppUserId? AppUserId,
    TestTakenRecordId TestTakenRecordId,
    DateTime CreatedOn,
    //
    string Text,
    bool WasLeftAnonymously
) : BaseFeedbackForTestLeftEvent(TestId, AppUserId, TestTakenRecordId, CreatedOn);