using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;

namespace TestCreationService.Domain.TestAggregate.general_format.events;

public record class GeneralTestResultDeletedEvent(
    TestId TestId,
    GeneralTestResultId ResultId
) : IDomainEvent;