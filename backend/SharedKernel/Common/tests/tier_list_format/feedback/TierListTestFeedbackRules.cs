using SharedKernel.Common.errors;

namespace SharedKernel.Common.tests.tier_list_format.feedback;

public static class TierListTestFeedbackRules
{
    public const int
        MaxAccompanyingTextLength = 500,
        MaxPossibleFeedbackLength = 500;

    public static ErrOrNothing CheckAccompanyingTextForErrs(string str) {
        int len = string.IsNullOrWhiteSpace(str) ? 0 : str.Length;
        if (len == 0) {
            return Err.ErrFactory.InvalidData(
                message: "Accompanying text cannot be empty"
            );
        }

        if (len > MaxAccompanyingTextLength) {
            return Err.ErrFactory.InvalidData(
                $"Accompanying text is too long. Maximum length is {MaxAccompanyingTextLength}",
                details: $"Maximum length is {MaxAccompanyingTextLength}. Current length is {len}"
            );
        }

        return ErrOrNothing.Nothing;
    }

    public static ErrOrNothing CheckMaxFeedbackLengthForErrs(ushort value) {
        if (value == 0) {
            return Err.ErrFactory.InvalidData("Maximum feedback length must be greater than 0");
        }

        if (value > MaxPossibleFeedbackLength) {
            return Err.ErrFactory.InvalidData(
                $"Maximum feedback length cannot be greater than {MaxPossibleFeedbackLength}"
            );
        }

        return ErrOrNothing.Nothing;
    }
}