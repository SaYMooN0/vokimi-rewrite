using ApiShared.interfaces;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.tiers;

public class AddNewTierListTestTierRequest : IRequestWithValidationNeeded
{
    public string TierName { get; init; }

    public RequestValidationResult Validate() =>
        TierListTestTiersRules.CheckIfStringCorrectTierName(TierName).IsErr(out var err)
            ? err
            : RequestValidationResult.Success;
}
