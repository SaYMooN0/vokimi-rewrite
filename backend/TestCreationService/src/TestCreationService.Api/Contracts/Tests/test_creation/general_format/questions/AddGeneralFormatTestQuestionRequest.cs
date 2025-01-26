using ApiShared.interfaces;
using SharedKernel.Common.general_test_questions;
using System.Text.Json.Serialization;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.questions;

internal record class AddGeneralFormatTestQuestionRequest(
    GeneralTestAnswersType AnswersType
) : IRequestWithValidationNeeded
{
    public RequestValidationResult Validate() {
        return RequestValidationResult.Success;
    }
}
