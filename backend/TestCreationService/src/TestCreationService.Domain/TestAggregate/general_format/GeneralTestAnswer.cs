using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format_tests;
using System.Collections.Immutable;

namespace TestCreationService.Domain.TestAggregate.general_format;

public class GeneralTestAnswer : Entity<GeneralTestAnswerId>
{
    public GeneralTestQuestionId QuestionId { get; init; }
    public GeneralTestAnswerTypeSpecificData TypeSpecificData { get; private set; }
    protected virtual HashSet<GeneralTestResultId> _relatedResultIds { get; init; } = [];
    public ImmutableHashSet<GeneralTestResultId> RelatedResultIds => _relatedResultIds
        .Order()
        .ToImmutableHashSet();


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
    //public ErrOrNothing AddRelatedResult(GeneralTestResultId resultId) {
    //    //no more than
    //    return ErrOrNothing.Nothing;

    //}
    //public void RemoveRelatedResult(GeneralTestResultId resultId) {
    //    _relatedResultIds.Remove(resultId);
    //}

}
