using SharedKernel.Common.domain;

namespace TestCreationService.Domain.GeneralTestQuestionAggregate.events;

public record class GeneralTestQuestionDeletedEvent(
    GeneralTestQuestionId QuestionId
) : IDomainEvent;

