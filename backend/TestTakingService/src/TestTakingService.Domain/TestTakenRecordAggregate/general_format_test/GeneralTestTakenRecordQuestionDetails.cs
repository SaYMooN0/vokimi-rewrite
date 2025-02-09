using System.Collections.Immutable;
using SharedKernel.Common.domain;
using TestTakingService.Domain.Common;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Domain.TestTakenRecordAggregate.general_format_test;

public class GeneralTestTakenRecordQuestionDetails : Entity<GeneralTestTakenRecordQuestionDetailsId>
{
    private GeneralTestTakenRecordQuestionDetails() { }
    private GeneralTestQuestionId QuestionId { get; init; }
    public ImmutableArray<GeneralTestAnswerId> ChosenAnswerIds { get; init; }
    public TimeSpan TimeOnQuestionSpent { get; init; }

    public static GeneralTestTakenRecordQuestionDetails CreateNew(
        GeneralTestQuestionId questionId,
        IEnumerable<GeneralTestAnswerId> chosenAnswerIds,
        TimeSpan timeOnQuestionSpent
    ) => new() {
        Id = GeneralTestTakenRecordQuestionDetailsId.CreateNew(),
        QuestionId = questionId,
        ChosenAnswerIds = chosenAnswerIds.ToImmutableArray(),
        TimeOnQuestionSpent = timeOnQuestionSpent
    };
}