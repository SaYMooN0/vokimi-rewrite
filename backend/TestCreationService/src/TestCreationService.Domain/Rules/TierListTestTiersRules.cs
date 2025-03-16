using SharedKernel.Common.errors;

namespace TestCreationService.Domain.Rules;

public static class TierListTestTiersRules
{
    private const int
        MinTierNameLen = 1,
        MaxTierNameLen = 60,
        MaxTierDescriptionLen = 120;

    private const ushort
        MinItemsLimitCount = 1,
        MaxItemsLimitCount = TierListTestRules.TestMaxItemsCount;

    public static ErrOrNothing CheckIfStringCorrectTierName(string name) {
        int len = string.IsNullOrWhiteSpace(name) ? 0 : name.Length;
        if (len < MinTierNameLen) {
            return Err.ErrFactory.InvalidData(
                $"Tier name is too short. Minimum allowed length is {MinTierNameLen} characters",
                details: $"Current length: {len}"
            );
        }

        if (len > MaxTierNameLen) {
            return Err.ErrFactory.InvalidData(
                $"Tier name is too long. Maximum allowed length is {MaxTierNameLen} characters",
                details: $"Current length: {len}"
            );
        }

        return ErrOrNothing.Nothing;
    }

    public static ErrOrNothing CheckIfStringCorrectTierDescription(string? description) {
        int len = string.IsNullOrWhiteSpace(description) ? 0 : description.Length;
        if (len > MaxTierDescriptionLen) {
            return Err.ErrFactory.InvalidData(
                $"Tier description is too long. Maximum allowed length is {MaxTierDescriptionLen} characters",
                details: $"Current length: {len}"
            );
        }

        return ErrOrNothing.Nothing;
    }

    public static ErrOrNothing CheckTierItemsCountLimit(ushort? maxItemsCountLimit) {
        if (maxItemsCountLimit is null) {
            return ErrOrNothing.Nothing;
        }

        if (maxItemsCountLimit.Value < MinItemsLimitCount || maxItemsCountLimit.Value > MaxItemsLimitCount) {
            return Err.ErrFactory.InvalidData(
                $"Incorrect items count limit. Value must be between {MinItemsLimitCount} and {MaxItemsLimitCount}",
                details: $"Current count limit: {maxItemsCountLimit}"
            );
        }

        return ErrOrNothing.Nothing;
    }
}