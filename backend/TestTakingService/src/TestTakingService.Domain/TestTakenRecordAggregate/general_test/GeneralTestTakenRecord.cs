using System.Collections.Immutable;
using SharedKernel.Common.domain;
using SharedKernel.Common.tests;
using TestTakingService.Domain.Common;
using TestTakingService.Domain.Common.general_test_taken_data;
using TestTakingService.Domain.TestFeedbackRecordAggregate.events;

namespace TestTakingService.Domain.TestTakenRecordAggregate.general_test;

public class GeneralTestTakenRecord : BaseTestTakenRecord
{
    private GeneralTestTakenRecord() { }
    public override TestFormat TestFormat => TestFormat.General;
    public GeneralTestResultId ReceivedResultId { get; init; }
    public ImmutableArray<GeneralTestTakenRecordQuestionDetails> QuestionDetails { get; init; }

    public static GeneralTestTakenRecord CreateNew(
        AppUserId? userId,
        TestId testId,
        DateTime testTakingStart,
        DateTime testTakingEnd,
        //general record specific
        GeneralTestResultId receivedResultId,
        IEnumerable<GeneralTestTakenRecordQuestionDetails> questionDetails
    ) {
        GeneralTestTakenRecord record = new() {
            Id = TestTakenRecordId.CreateNew(),
            UserId = userId,
            TestId = testId,
            TestTakingStart = testTakingStart,
            TestTakingEnd = testTakingEnd,
            ReceivedResultId = receivedResultId,
            QuestionDetails = questionDetails.ToImmutableArray()
        };

        return record;
    }
}