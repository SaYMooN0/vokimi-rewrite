using SharedKernel.Common.domain;

namespace TestCreationService.Domain.TestAggregate.general_format.events;

public record class GeneralTestResultDeletedEvent(
    TestId TestId,
    GeneralTestResultId ResultId
) : IDomainEvent;