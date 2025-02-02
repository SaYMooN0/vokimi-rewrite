using SharedKernel.Common.general_test_questions;
using TestCreationService.Domain.GeneralTestQuestionAggregate;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.answers;

internal record class GeneralFormatTestAnswerInfoResponse(
    string AnswerId,
    ushort Order,
    GeneralTestAnswersType Type,
    Dictionary<string, string> TypeSpecificData,
    string[] RelatedResultIds
)
{
    public static GeneralFormatTestAnswerInfoResponse Create(GeneralTestAnswer answer, ushort order) => new(
        answer.Id.ToString(),
        order,
        answer.TypeSpecificData.MatchingEnumType,
        answer.TypeSpecificData.ToDictionary(),
        answer.GetRelatedResultIds().Select(r => r.ToString()).ToArray()
    );
}
