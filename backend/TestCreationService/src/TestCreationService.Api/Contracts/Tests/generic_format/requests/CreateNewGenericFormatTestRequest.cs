using ApiShared.interfaces;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Api.Contracts.Tests.generic_format.requests;

public record class CreateNewGenericFormatTestRequest(
    string Name,
    string[] EditorIds
) : IRequestWithValidationNeeded
{
    public RequestValidationResult Validate() {
        int nameLength = string.IsNullOrWhiteSpace(Name) ? 0 : Name.Length;
        if (nameLength > TestRules.MaxNameLength) {
            return Err.ErrFactory.InvalidData(
                $"Test name is too long. Maximum length of the test name cannot be more than {TestRules.MaxNameLength}. Current length is {nameLength}"
            );
        }
        if (nameLength < TestRules.MinNameLength) {
            return Err.ErrFactory.InvalidData(
                $"Test name is too short. Minimum length of the test name cannot be less than {TestRules.MinNameLength}. Current length is {nameLength}"
            );
        }
        //check if editor ids is null
        if (EditorIds.Any(eId => !Guid.TryParse(eId, out var _))) {
            return Err.ErrFactory.InvalidData(
                "Editors was not saved correctly. Please try again.",
                details: "If it does not help you can create test without additional editors and add them later"
            );
        }
        return RequestValidationResult.Success;
    }
}
