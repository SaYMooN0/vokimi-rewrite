using ApiShared.interfaces;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Common.rules;

namespace TestCreationService.Api.Contracts.Tests.test_creation.formats_shared;

internal class UpdateTestMainInfoRequest : IRequestWithValidationNeeded
{
    public string TestName { get; init; }
    public string Description { get; init; }
    public Language Language { get; init; }
    public RequestValidationResult Validate() {
        int nameLen = string.IsNullOrWhiteSpace(TestName) ? 0 : TestName.Length;
        if (nameLen < TestRules.MinNameLength) {
            return Err.ErrFactory.InvalidData(
                $"Test name is too short. Minimum test name length is {TestRules.MinNameLength}",
                details: $"Minimum test name length is {TestRules.MinNameLength}. Current length is {nameLen}"
            );
        } else if (nameLen > TestRules.MaxNameLength) {
            return Err.ErrFactory.InvalidData(
                $"Test name is too long. Maximum test name length is {TestRules.MaxNameLength}",
                details: $"Maximum test name length is {TestRules.MaxNameLength}. Current length is {nameLen}"
            );
        }
        int descriptionLen = string.IsNullOrWhiteSpace(Description) ? 0 : Description.Length;
        if (descriptionLen > TestRules.MaxTestDescriptionLength) {
            return Err.ErrFactory.InvalidData(
                $"Test description cannot be longer then {TestRules.MaxTestDescriptionLength}. Current length is {descriptionLen}",
                details: "Test description is too long"
            );
        }
        return RequestValidationResult.Success;
    }
}
