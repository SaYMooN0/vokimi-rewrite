using SharedKernel.Common.domain;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.TestTakenRecordAggregate.general_test.events;

public record class GeneralTestTakenEvent(
    TestId TestId,
    AppUserId UserId,
    DateTime TestTakingStart,
    DateTime TestTakingEnd,
    GeneralTestResultId ReceivedResultId,
    Dictionary<GeneralTestQuestionId, GeneralTestTakenEventQuestionDetails> QuestionDetails,
    GeneralTestTakenFeedbackData? Feedback
) : IDomainEvent;

public record class GeneralTestTakenEventQuestionDetails(
    IEnumerable<GeneralTestAnswerId> ChosenAnswerIds,
    TimeSpan TimeOnQuestionSpent
);