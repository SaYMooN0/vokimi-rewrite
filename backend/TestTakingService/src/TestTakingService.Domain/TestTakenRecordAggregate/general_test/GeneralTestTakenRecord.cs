using System.Collections.Immutable;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.TestTakenRecordAggregate.general_test;

public class GeneralTestTakenRecord : BaseTestTakenRecord
{
    private GeneralTestTakenRecord() { }
    public override TestFormat TestFormat => TestFormat.General;
    public GeneralTestResultId ReceivedResultId { get; }
    public IReadOnlyCollection<GeneralTestTakenRecordQuestionDetails> QuestionDetails { get; private init; }

    public GeneralTestTakenRecord(
        TestTakenRecordId id,
        AppUserId? userId,
        TestId testId,
        DateTime testTakingStart,
        DateTime testTakingEnd,
        //general test taken record specific
        GeneralTestResultId receivedResultId,
        ImmutableArray<GeneralTestTakenRecordQuestionDetails> questionDetails
    ) {
        Id = id;
        UserId = userId;
        TestId = testId;
        TestTakingStart = testTakingStart;
        TestTakingEnd = testTakingEnd;
        ReceivedResultId = receivedResultId;
        QuestionDetails = questionDetails;
    }
}