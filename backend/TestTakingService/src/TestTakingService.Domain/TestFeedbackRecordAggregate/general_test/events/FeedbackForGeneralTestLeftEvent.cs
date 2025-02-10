using SharedKernel.Common.domain;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.TestFeedbackRecordAggregate.general_test.events;

public record FeedbackForGeneralTestLeftEvent(
    TestId TestId,
    AppUserId UserId,
    TestTakenRecordId TestTakenRecordId,
    DateTime CreatedOn,
    string Text,
    bool WasLeftAnonymously
) : IDomainEvent;