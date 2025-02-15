using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using System.Collections.Immutable;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Domain.GeneralTestQuestionAggregate;

public class GeneralTestAnswer : Entity<GeneralTestAnswerId>
{
    private GeneralTestAnswer() { }
    public GeneralTestQuestionId QuestionId { get; init; }
    public GeneralTestAnswerTypeSpecificData TypeSpecificData { get; private set; }
    private HashSet<GeneralTestResultId> _relatedResultIds { get; set; } = [];
    public ImmutableHashSet<GeneralTestResultId> GetRelatedResultIds() => _relatedResultIds.ToImmutableHashSet();
    public static ErrOr<GeneralTestAnswer> CreateNew(
        GeneralTestQuestionId questionId,
        GeneralTestAnswerTypeSpecificData typeSpecificData,
        HashSet<GeneralTestResultId> relatedResultIds
    ) {
        if (relatedResultIds.Count > GeneralFormatTestRules.MaxRelatedResultsForAnswer) {
            return Err.ErrFactory.InvalidData(
                "Too many related results selected",
                details: $"Maximum possible number of related results: {GeneralFormatTestRules.MaxRelatedResultsForAnswer}. Results selected: {relatedResultIds.Count}"
            );
        }
        return new GeneralTestAnswer() {
            Id = GeneralTestAnswerId.CreateNew(),
            QuestionId = questionId,
            TypeSpecificData = typeSpecificData,
            _relatedResultIds = relatedResultIds
        };

    }
    public ErrOrNothing Update(
        GeneralTestAnswerTypeSpecificData newData,
        HashSet<GeneralTestResultId> relatedResultIds
    ) {
        if (TypeSpecificData.MatchingEnumType != newData.MatchingEnumType) {
            return new Err(
                "New type specific data doesn't match with the type of the answer",
                details: $"Previous type: {TypeSpecificData.MatchingEnumType}, new type: {newData.MatchingEnumType}"
            );
        }
        if (relatedResultIds.Count > GeneralFormatTestRules.MaxRelatedResultsForAnswer) {
            return Err.ErrFactory.InvalidData(
                "Too many related results selected",
                details: $"Maximum possible number of related results: {GeneralFormatTestRules.MaxRelatedResultsForAnswer}. Results selected: {relatedResultIds.Count}"
            );
        }
        TypeSpecificData = newData;
        _relatedResultIds = relatedResultIds;
        return ErrOrNothing.Nothing;
    }
    public void RemoveRelatedResultId(GeneralTestResultId resultId) =>
        _relatedResultIds.Remove(resultId);
}
