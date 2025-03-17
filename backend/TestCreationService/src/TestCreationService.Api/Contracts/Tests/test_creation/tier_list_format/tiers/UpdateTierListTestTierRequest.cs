using ApiShared.interfaces;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.tiers;

internal class UpdateTierListTestTierRequest : IRequestWithValidationNeeded
{
    public string Name { get; init; }
    public string? Description { get; init; } = null;
    public ushort? MaxItemsCountLimit { get; init; } = null;
    public TierListTestTierStylesContract Styles { get; init; } = null;

    public RequestValidationResult Validate() {
        if (
            TierListTestTiersRules.CheckIfStringCorrectTierName(Name).IsErr(out var err)
            || TierListTestTiersRules.CheckIfStringCorrectTierDescription(Description).IsErr(out err)
            || TierListTestTiersRules.CheckTierItemsCountLimit(MaxItemsCountLimit).IsErr(out err)
        ) {
            return err;
        }

        if (Styles is null) {
            return new Err("Styles for the tier are not provided");
        }

        if (Styles.ParseToTierStyles().IsErr(out err)) {
            return err;
        }

        return RequestValidationResult.Success;
    }

}