using SharedKernel.Common.domain;
using TestTakingService.Domain.Common;
using TestTakingService.Domain.Common.general_test_taken_data;

namespace TestTakingService.Domain.TestTakenRecordAggregate.events;

public record class GeneralBaseTestTakenEvent(
    TestTakenRecordId TestTakenRecordId,
    TestId TestId,
    AppUserId? AppUserId,
    DateTime TestTakingStart,
    DateTime TestTakingEnd,
    GeneralTestResultId ReceivedResultId,
    Dictionary<GeneralTestQuestionId, GeneralTestTakenEventQuestionDetails> QuestionDetails
) : BaseTestTakenEvent(TestTakenRecordId, TestId, AppUserId, TestTakingStart, TestTakingEnd);

public record class GeneralTestTakenEventQuestionDetails(
    IEnumerable<GeneralTestAnswerId> ChosenAnswerIds,
    TimeSpan TimeOnQuestionSpent
);