using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.general_test_questions;

namespace TestCreationService.Domain.GeneralTestQuestionAggregate.events;

public record class NewGeneralTestQuestionAddedEvent(
    TestId TestId,
    GeneralTestQuestionId QuestionId,
    GeneralTestAnswersType AnswersType
) : IDomainEvent;

