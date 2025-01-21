using SharedKernel.Common.errors;

namespace TestCreationService.Domain.Rules;

public static class TestRules
{
    public const int
        MinNameLength = 8,
        MaxNameLength = 255;
    public const int MaxTestEditorsCount = 10;

    public const int MaxTestDescriptionLength = 511;
    public static ErrOrNothing CheckTestNameForErrs(string str) {
        int len = string.IsNullOrWhiteSpace(str) ? 0 : str.Length;
        if (len < TestRules.MinNameLength) {
            return Err.ErrFactory.InvalidData(
                $"Test name is too short. Minimum length is {TestRules.MinNameLength} characters",
                details: $"Current length: {len}. Minimum required: {TestRules.MinNameLength}"
            );
        }
        if (len > TestRules.MaxNameLength) {
            return Err.ErrFactory.InvalidData(
                $"Test name is too long. Maximum length is {TestRules.MaxNameLength} characters",
                details: $"Current length: {len}. Maximum allowed: {TestRules.MaxNameLength}"
            );
        }
        return ErrOrNothing.Nothing;
    }
    public static ErrOrNothing CheckDescriptionForErrs(string str) {
        int len = string.IsNullOrWhiteSpace(str) ? 0 : str.Length;
        if (len > TestRules.MaxTestDescriptionLength) {
            return Err.ErrFactory.InvalidData(
                $"Test description is too long. Maximum length is {TestRules.MaxTestDescriptionLength} characters",
                details: $"Current length: {len}. Maximum allowed: {TestRules.MaxTestDescriptionLength}"
            );
        }
        return ErrOrNothing.Nothing;
    }

}
