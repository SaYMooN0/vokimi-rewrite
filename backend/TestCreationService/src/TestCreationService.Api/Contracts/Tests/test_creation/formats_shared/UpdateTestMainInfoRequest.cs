using ApiShared.interfaces;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Api.Contracts.Tests.test_creation.formats_shared;

internal class UpdateTestMainInfoRequest : IRequestWithValidationNeeded
{
    public string TestName { get; init; }
    public string Description { get; init; }
    public Language Language { get; init; }
    public RequestValidationResult Validate() {
        if (TestRules.CheckTestNameForErrs(TestName).IsErr(out var err)) {
            return err;
        }
        if (TestRules.CheckDescriptionForErrs(Description).IsErr(out err)) {
            return err;
        }
        return RequestValidationResult.Success;
    }
}
