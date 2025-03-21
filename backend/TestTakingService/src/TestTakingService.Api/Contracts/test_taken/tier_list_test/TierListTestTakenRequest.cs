using ApiShared.interfaces;
using SharedKernel.Common.errors;
using TestTakingService.Api.Contracts.test_taken.test_formats_shared;

namespace TestTakingService.Api.Contracts.test_taken.tier_list_test;

internal class TierListTestTakenRequest : TestTakenRequest
{
    public override DateTime StartDateTime { get; init; }
    public override DateTime EndDateTime { get; init; }

    public override RequestValidationResult Validate() {
        ErrList errs = new();

        errs.AddPossibleErr(base.ValidateStartAndEndDateTine());

        return errs;
    }
}