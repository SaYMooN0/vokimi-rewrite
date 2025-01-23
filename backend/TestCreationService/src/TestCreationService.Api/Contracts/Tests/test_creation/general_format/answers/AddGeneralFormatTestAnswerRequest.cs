using ApiShared.interfaces;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.answers;

internal class AddGeneralFormatTestAnswerRequest : IRequestWithValidationNeeded
{
    public RequestValidationResult Validate() {
        return RequestValidationResult.Success;
    }
}
