using ApiShared.interfaces;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.questions;

public record class GeneralTestQuestionUpdateRequest : IRequestWithValidationNeeded
{
    public RequestValidationResult Validate() {
        return RequestValidationResult.Success;
    }
}
