using ApiShared.interfaces;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Api.Contracts.Tests.test_creation.formats_shared.update_editors;

public record class UpdateTestEditorsRequest(
    string[] EditorIds
) : IRequestWithValidationNeeded
{
    public RequestValidationResult Validate() {
        if (EditorIds.Length > TestRules.MaxTestEditorsCount) {
            return Err.ErrFactory.InvalidData(
                message: $"Too many editors selected. Maximum number of editors is {TestRules.MaxTestEditorsCount}",
                details: $"Maximum number of editors is {TestRules.MaxTestEditorsCount}. Number of selected is {EditorIds.Length}");
        }
        if (EditorIds.Any(id => !Guid.TryParse(id, out var _))) {
            return Err.ErrFactory.InvalidData(
                "Unable to update test editors. Please try again later",
                details: "Invalid editor id format. If you are trying to change multiple editors at once, please try to change them one by one."
            );
        }
        return RequestValidationResult.Success;
    }
}
