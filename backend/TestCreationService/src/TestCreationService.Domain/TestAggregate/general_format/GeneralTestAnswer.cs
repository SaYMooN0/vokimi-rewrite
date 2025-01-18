using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format_tests;

namespace TestCreationService.Domain.TestAggregate.general_format;

public class GeneralTestAnswer : Entity
{
    protected override EntityId EntityId => Id;

    public GeneralTestAnswerId Id { get; init; }
    public GeneralTestQuestionId QuestionId { get; init; }
    public GeneralTestAnswerTypeSpecificData TypeSpecificData { get; private set; }
    protected virtual List<GeneralTestResult> _relatedResults { get; init; } = [];
    public IReadOnlyList<GeneralTestResult> RelatedResults => _relatedResults
        .OrderBy(r=>r.Id)
        .ToList()
        .AsReadOnly();


    public ErrOrNothing Update(GeneralTestAnswerTypeSpecificData newData) {
        if (TypeSpecificData.MatchingEnumType != newData.MatchingEnumType) {
            return new Err(
                "New type specific data doesn't match with the type of the answer",
                details: $"Previous type: {TypeSpecificData.MatchingEnumType}, new type: {newData.MatchingEnumType}"
            );
        }
        TypeSpecificData = newData;
        return ErrOrNothing.Nothing;
    }

}
