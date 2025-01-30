using ApiShared.interfaces;
using SharedKernel.Common.errors;

namespace TestCreationService.Api.Contracts.Tests.test_creation.formats_shared;

public record class UpdateTestTagsRequest(
    string[] Tags
) : IRequestWithValidationNeeded
{
    public RequestValidationResult Validate() {
        if (Tags is null) {
            return Err.ErrFactory.InvalidData("Tags list is not set");
        }
        if (Tags.Any(string.IsNullOrWhiteSpace)) {
            return Err.ErrFactory.InvalidData("New tag list contains empty tags. Tags with no value are not allowed");
        }
        return RequestValidationResult.Success;
    }
}
