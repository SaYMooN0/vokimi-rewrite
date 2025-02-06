using System.Collections.Immutable;
using SharedKernel.Common.domain;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;

namespace TestTakingService.Domain.TestAggregate.general_format;

public class GeneralTestAnswer : Entity<GeneralTestAnswerId>
{
    private GeneralTestAnswer() { }
    private GeneralTestQuestionId _questionId { get; init; }
    public ushort OrderInQuestion { get; init; }
    public GeneralTestAnswerTypeSpecificData TypeSpecificData { get; init; }
    protected virtual ImmutableArray<GeneralTestResult> _relatedResults { get; init; }

    public GeneralTestAnswer(
        GeneralTestAnswerId id,
        GeneralTestQuestionId questionId,
        GeneralTestAnswerTypeSpecificData typeSpecificData,
        ImmutableArray<GeneralTestResult> relatedResults
    ) {
        Id = id;
        _questionId = questionId;
        TypeSpecificData = typeSpecificData;
        _relatedResults = relatedResults;
    }
}