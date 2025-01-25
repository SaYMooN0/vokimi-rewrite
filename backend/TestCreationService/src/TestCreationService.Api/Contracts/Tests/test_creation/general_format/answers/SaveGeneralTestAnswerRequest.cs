using ApiShared.interfaces;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format_tests;
using SharedKernel.Common.tests.general_format_tests.answer_type_specific_data;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.answers;

internal class SaveGeneralTestAnswerRequest : IRequestWithValidationNeeded
{
    public GeneralTestAnswersType Type { get; init; }
    public Dictionary<string, string> TypeSpecificData { get; init; } = [];
    public static GeneralFormatTestAnswerInfoResponse Create(GeneralTestAnswer answer, ushort order) => new() {
        AnswerId = answer.Id.ToString(),
        Order = order,
        Type = answer.TypeSpecificData.MatchingEnumType,
        TypeSpecificData = answer.TypeSpecificData.ToDictionary()
    };

    public RequestValidationResult Validate() {
        if (ParsedAnswerData().IsErr(out var err)) {
            return err;
        }
        return RequestValidationResult.Success;
    }
    public ErrOr<GeneralTestAnswerTypeSpecificData> ParsedAnswerData() =>
        GeneralTestAnswerTypeSpecificData.CreateFromDictionary(Type, TypeSpecificData);
}
