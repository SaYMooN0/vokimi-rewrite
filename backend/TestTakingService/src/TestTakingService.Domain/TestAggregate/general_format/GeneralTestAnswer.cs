using System.Collections.Immutable;
using SharedKernel.Common.domain;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;

namespace TestTakingService.Domain.TestAggregate.general_format;

public class GeneralTestAnswer : Entity<GeneralTestAnswerId>
{
    private GeneralTestAnswer() { }

    public GeneralTestAnswer(
        GeneralTestAnswerId id,
        GeneralTestAnswerTypeSpecificData typeSpecificData,
        ImmutableArray<GeneralTestResult> relatedResults
    ) {
        Id = id;
        TypeSpecificData = typeSpecificData;
        RelatedResults = relatedResults;
    }

    public ushort OrderInQuestion { get; init; }
    public GeneralTestAnswerTypeSpecificData TypeSpecificData { get; init; }
    protected ImmutableArray<GeneralTestResult> RelatedResults { get; init; }
}