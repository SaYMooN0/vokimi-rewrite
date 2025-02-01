using ApiShared.interfaces;
using TestCreationService.Domain.Rules;
using SharedKernel.Common.errors;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.results;

public record class UpdateGeneralTestResultRequest(
    string Name,
    string Text,
    string Image
) : IRequestWithValidationNeeded
{
    public RequestValidationResult Validate() {
        ErrList errs = new();
        errs.AddPossibleErr(GeneralFormatTestRules.CheckResultNameForErrs(Name));
        errs.AddPossibleErr(GeneralFormatTestRules.CheckResultTextForErrs(Text));
        return errs;
    }
}
