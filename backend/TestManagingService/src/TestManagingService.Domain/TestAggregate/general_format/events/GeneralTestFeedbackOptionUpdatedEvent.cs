using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.general_format;

namespace TestManagingService.Domain.TestAggregate.general_format.events;

public record GeneralTestFeedbackOptionUpdatedEvent(
    TestId TestId,
    GeneralTestFeedbackOption NewFeedbackOption
) : IDomainEvent;