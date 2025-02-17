using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;

namespace TestTakingService.Domain.TestAggregate.general_format;

public class GeneralTestAnswer : Entity<GeneralTestAnswerId>
{
    private GeneralTestAnswer() { }

    public GeneralTestAnswer(
        GeneralTestAnswerId id,
        GeneralTestAnswerTypeSpecificData typeSpecificData,
        IReadOnlyCollection<GeneralTestResult> relatedResults
    ) {
        Id = id;
        TypeSpecificData = typeSpecificData;
        RelatedResults = relatedResults;
    }

    public ushort OrderInQuestion { get; init; }
    public GeneralTestAnswerTypeSpecificData TypeSpecificData { get; init; }
    protected IReadOnlyCollection<GeneralTestResult> RelatedResults { get; init; }

    public IReadOnlyCollection<GeneralTestResultId> RelatedResultIds => RelatedResults
        .Select(r => r.Id)
        .ToArray();
}