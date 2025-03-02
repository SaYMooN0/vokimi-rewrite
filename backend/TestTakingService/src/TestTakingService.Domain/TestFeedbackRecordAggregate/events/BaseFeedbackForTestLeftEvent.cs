using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.TestFeedbackRecordAggregate.events;

public abstract record class BaseFeedbackForTestLeftEvent(
    TestId TestId,
    AppUserId? AuthorId,
    DateTime CreatedOn
) : IDomainEvent;