using SharedKernel.Common.errors;

namespace TestCreationService.Domain.Rules;

public static class TierListTestTiersRules
{
    public const int
        MinNameLength = 4,
        MaxNameLength = 32;
    public static ErrOrNothing CheckTierNameForErrs(string text) {
        int len = string.IsNullOrWhiteSpace(text) ? 0 : text.Length;

        if (len < MinNameLength) {
            return Err.ErrFactory.InvalidData(
                $"Tier name is too short. Minimum length is {MinNameLength} characters",
                details: $"Current length: {len}. Minimum required: {MinNameLength}"
            );
        }

        if (len > MaxNameLength) {
            return Err.ErrFactory.InvalidData(
                $"Tier name is too long. Maximum length is {MaxNameLength} characters",
                details: $"Current length: {len}. Maximum allowed: {MaxNameLength}"
            );
        }

        return ErrOrNothing.Nothing;
    }
}