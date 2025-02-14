using SharedKernel.Common.common_enums;
using SharedKernel.Common.errors;

namespace TestCreationService.Domain.Rules;

public static class TestRules
{
    public const int
        MinNameLength = 8,
        MaxNameLength = 255;
    public const int MaxTestEditorsCount = 10;

    public const int MaxTestDescriptionLength = 500;
    public static ErrOrNothing CheckTestNameForErrs(string str) {
        int len = string.IsNullOrWhiteSpace(str) ? 0 : str.Length;
        if (len < MinNameLength) {
            return Err.ErrFactory.InvalidData(
                $"Test name is too short. Minimum length is {MinNameLength} characters",
                details: $"Current length: {len}. Minimum required: {MinNameLength}"
            );
        }
        if (len > MaxNameLength) {
            return Err.ErrFactory.InvalidData(
                $"Test name is too long. Maximum length is {MaxNameLength} characters",
                details: $"Current length: {len}. Maximum allowed: {MaxNameLength}"
            );
        }
        return ErrOrNothing.Nothing;
    }
    public static ErrOrNothing CheckDescriptionForErrs(string str) {
        int len = string.IsNullOrWhiteSpace(str) ? 0 : str.Length;
        if (len > MaxTestDescriptionLength) {
            return Err.ErrFactory.InvalidData(
                $"Test description is too long. Maximum length is {MaxTestDescriptionLength} characters",
                details: $"Current length: {len}. Maximum allowed: {MaxTestDescriptionLength}"
            );
        }
        return ErrOrNothing.Nothing;
    }
    public static ErrOrNothing CheckCoverStringForErrs(string coverImg) {
        if (string.IsNullOrWhiteSpace(coverImg) || coverImg.Length == 0) {
            return Err.ErrFactory.InvalidData("Cover image cannot be empty");
        }
        return ErrOrNothing.Nothing;
    }
}
