using SharedKernel.Common.general_test_questions;
using TestCreationService.Domain.GeneralTestQuestionAggregate;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.answers;

internal class GeneralFormatTestAnswerInfoResponse
{
    public string AnswerId { get; init; }
    public ushort Order { get; init; }
    public GeneralTestAnswersType Type { get; init; }
    public Dictionary<string, string> TypeSpecificData { get; init; } = [];
    public static GeneralFormatTestAnswerInfoResponse Create(GeneralTestAnswer answer, ushort order) => new() {
        AnswerId = answer.Id.ToString(),
        Order = order,
        Type = answer.TypeSpecificData.MatchingEnumType,
        TypeSpecificData = answer.TypeSpecificData.ToDictionary()
    };
}
