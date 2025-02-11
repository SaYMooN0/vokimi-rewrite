using SharedKernel.Common.domain;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.AppUserAggregate.events;

public record class TestFeedbackLeftByAuthenticatedUserEvent(
    AppUserId AppUserId,
    TestFeedbackRecordId FeedbackRecordId
) : IDomainEvent;