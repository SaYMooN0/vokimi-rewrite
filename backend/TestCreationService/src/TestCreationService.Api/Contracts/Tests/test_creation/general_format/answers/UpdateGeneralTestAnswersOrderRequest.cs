using ApiShared.interfaces;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.answers;

internal class UpdateGeneralTestAnswersOrderRequest : IRequestWithValidationNeeded
{
    public RequestValidationResult Validate() {
        return RequestValidationResult.Success;
    }
}
