using SharedKernel.Common.domain;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.TestFeedbackRecordAggregate.events;

public abstract record class BaseFeedbackForTestLeftEvent(
    TestFeedbackRecordId FeedbackRecordId,
    TestId TestId,
    AppUserId? AppUserId,
    TestTakenRecordId TestTakenRecordId,
    DateTime CreatedOn
) : IDomainEvent;