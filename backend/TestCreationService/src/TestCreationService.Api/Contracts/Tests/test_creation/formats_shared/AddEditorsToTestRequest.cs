using ApiShared.interfaces;
using SharedKernel.Common.errors;

namespace TestCreationService.Api.Contracts.Tests.test_creation.formats_shared;

public record class AddEditorsToTestRequest(
    string[] EditorIds
) : IRequestWithValidationNeeded
{
    public RequestValidationResult Validate() {
        if (EditorIds.Length == 0) {
            return Err.ErrFactory.InvalidData("Editors list is empty");
        }
        if (EditorIds.Any(id => !Guid.TryParse(id, out var _))) {
            return Err.ErrFactory.InvalidData(
                "Unable to add editors to test. Please try again later",
                details: "Invalid editor id format. If you are trying to add multiple editors at once, please try to add them one by one."
            );
        }
        return RequestValidationResult.Success;
    }
}
