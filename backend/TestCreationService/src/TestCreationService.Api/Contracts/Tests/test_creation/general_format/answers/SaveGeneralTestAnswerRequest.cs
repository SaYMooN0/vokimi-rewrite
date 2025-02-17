using ApiShared.interfaces;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.general_test_questions;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.answers;

internal class SaveGeneralTestAnswerRequest : IRequestWithValidationNeeded
{
    public GeneralTestAnswersType Type { get; init; }
    public Dictionary<string, string> TypeSpecificData { get; init; } = [];
    public string[] RelatedResults { get; init; } = [];
    public RequestValidationResult Validate() {
        if (ParsedRelatedResultIds().IsErr(out var err)) {
            return err;
        }
        if (ParsedAnswerData().IsErr(out err)) {
            return err;
        }
        return RequestValidationResult.Success;
    }
    public ErrOr<GeneralTestAnswerTypeSpecificData> ParsedAnswerData() =>
        GeneralTestAnswerTypeSpecificData.CreateFromDictionary(Type, TypeSpecificData);
    public ErrOr<HashSet<GeneralTestResultId>> ParsedRelatedResultIds() {
        var resultIds = new HashSet<GeneralTestResultId>();
        foreach (var result in RelatedResults) {
            if (!Guid.TryParse(result, out var guid)) {
                return Err.ErrFactory.InvalidData("Incorrect result id format", $"Given value: {result}");
            }
            resultIds.Add(new GeneralTestResultId(guid));
        }
        return resultIds;
    }
}
