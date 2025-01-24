using SharedKernel.Common.tests.general_format_tests;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.answers;

internal class GeneralFormatTestAnswerInfoResponse
{
    public string AnswerId { get; init; }
    public ushort Order { get; init; }
    public GeneralTestAnswersType Type { get; init; }
    public GeneralTestAnswerTypeSpecificData TypeSpecificData { get; init; }
    public static GeneralFormatTestAnswerInfoResponse Create(GeneralTestAnswer answer, ushort order) => new() {
        AnswerId = answer.Id.ToString(),
        Order = order,
        Type = answer.TypeSpecificData.MatchingEnumType,
        TypeSpecificData = answer.TypeSpecificData
    };
}
