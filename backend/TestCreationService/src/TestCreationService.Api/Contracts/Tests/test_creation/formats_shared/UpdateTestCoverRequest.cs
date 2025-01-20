using ApiShared.interfaces;
using SharedKernel.Common.errors;

namespace TestCreationService.Api.Contracts.Tests.test_creation.formats_shared;

internal class UpdateTestCoverRequest : IRequestWithValidationNeeded
{
    public string CoverImg { get; init; }
    public RequestValidationResult Validate() {
        if (string.IsNullOrWhiteSpace(CoverImg)) {
            return Err.ErrFactory.InvalidData(
                "Unable to update test cover. Please try again later",
                details: "Cover image cannot be empty"
            );
        }
        return RequestValidationResult.Success;
    }
}
