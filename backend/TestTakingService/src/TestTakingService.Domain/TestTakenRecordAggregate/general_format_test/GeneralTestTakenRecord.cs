using System.Collections.Immutable;
using SharedKernel.Common.domain;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.TestTakenRecordAggregate.general_format_test;

public class GeneralTestTakenRecord : BaseTestTakenRecord
{
    private GeneralTestTakenRecord() { }
    private GeneralTestResultId ReceivedResultId { get; init; }
    public ImmutableArray<GeneralTestTakenRecordQuestionDetails> QuestionDetails { get; init; }

    public GeneralTestTakenRecord(
        AppUserId userId,
        TestId testId,
        DateTime testTakingStart,
        DateTime testTakingEnd,
        //general record specific
        GeneralTestResultId receivedResultId,
        IEnumerable<GeneralTestTakenRecordQuestionDetails> questionDetails
    ) : base(
        userId, testId,
        testTakingStart: testTakingStart,
        testTakingEnd: testTakingEnd
    ) {
        Id = TestTakenRecordId.CreateNew();
        ReceivedResultId = receivedResultId;
        QuestionDetails = questionDetails.ToImmutableArray();
    }
}