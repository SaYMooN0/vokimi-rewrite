using System.Collections.Immutable;
using SharedKernel.Common.domain;
using TestTakingService.Domain.Common;
using TestTakingService.Domain.TestFeedbackRecordAggregate.general_test.events;

namespace TestTakingService.Domain.TestTakenRecordAggregate.general_test;

public class GeneralTestTakenRecord : BaseTestTakenRecord
{
    private GeneralTestTakenRecord() { }
    private GeneralTestResultId ReceivedResultId { get; init; }
    public ImmutableArray<GeneralTestTakenRecordQuestionDetails> QuestionDetails { get; init; }

    public static GeneralTestTakenRecord CreateNew(
        AppUserId userId,
        TestId testId,
        DateTime testTakingStart,
        DateTime testTakingEnd,
        //general record specific
        GeneralTestResultId receivedResultId,
        IEnumerable<GeneralTestTakenRecordQuestionDetails> questionDetails,
        GeneralTestTakenFeedbackData? feedbackData
    ) {
        GeneralTestTakenRecord record = new() {
            UserId = userId,
            TestId = testId,
            TestTakingStart = testTakingStart,
            TestTakingEnd = testTakingEnd,
            ReceivedResultId = receivedResultId,
            QuestionDetails = questionDetails.ToImmutableArray()
        };
        if (feedbackData is not null) {
            record._domainEvents.Add(new FeedbackForGeneralTestLeftEvent(
                testId, userId, record.Id, testTakingEnd,
                feedbackData.FeedbackText, feedbackData.LeftAnonymously
            ));
        }

        return record;
    }
}