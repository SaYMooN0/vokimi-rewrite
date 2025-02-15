using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;

namespace TestCreationService.Domain.GeneralTestQuestionAggregate.events;

public record class GeneralTestQuestionDeletedEvent(
    GeneralTestQuestionId QuestionId
) : IDomainEvent;

