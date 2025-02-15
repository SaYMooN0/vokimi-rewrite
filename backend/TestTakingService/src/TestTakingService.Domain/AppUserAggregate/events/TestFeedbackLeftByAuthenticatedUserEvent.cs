using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.AppUserAggregate.events;

public record class TestFeedbackLeftByAuthenticatedUserEvent(
    AppUserId AppUserId,
    TestFeedbackRecordId FeedbackRecordId
) : IDomainEvent;