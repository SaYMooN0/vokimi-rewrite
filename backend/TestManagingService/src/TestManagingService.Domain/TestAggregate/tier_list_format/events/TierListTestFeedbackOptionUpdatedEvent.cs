using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.tier_list_format.feedback;

namespace TestManagingService.Domain.TestAggregate.tier_list_format.events;

public record TierListTestFeedbackOptionUpdatedEvent(
    TestId TestId,
    TierListTestFeedbackOption NewFeedbackOption
) : IDomainEvent;