using SharedKernel.Common.EntityIds;
using SharedKernel.Common;

namespace TestCreationService.Domain.GeneralTestQuestionAggregate.events;

public record class GeneralTestQuestionDeletedEvent(
    GeneralTestQuestionId QuestionId
) : IDomainEvent;

