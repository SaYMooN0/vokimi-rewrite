using ApiShared.interfaces;
using SharedKernel.Common.tests.general_format_tests;
using System.Text.Json.Serialization;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.questions;

internal class AddGeneralFormatTestQuestionRequest : IRequestWithValidationNeeded
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GeneralTestAnswersType AnswersType { get; init; }
    public RequestValidationResult Validate() {
        return RequestValidationResult.Success;
    }
}
