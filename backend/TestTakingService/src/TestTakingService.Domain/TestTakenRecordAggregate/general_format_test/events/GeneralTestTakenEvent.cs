using SharedKernel.Common.domain;

namespace TestTakingService.Domain.TestTakenRecordAggregate.general_format_test.events;

public record class GeneralTestTakenEvent(
    TestId TestId,
    AppUserId UserId,
    DateTime TestTakingStart,
    DateTime TestTakingEnd,
    GeneralTestResultId ReceivedResultId,
    Dictionary<GeneralTestQuestionId, GeneralTestTakenEventQuestionDetails> QuestionDetails
) : IDomainEvent;

public record class GeneralTestTakenEventQuestionDetails(
    IEnumerable<GeneralTestAnswerId> ChosenAnswerIds,
    TimeSpan TimeOnQuestionSpent
);