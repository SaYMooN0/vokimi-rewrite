using SharedKernel.Common.errors;

namespace TestCreationService.Domain.Common.rules;

public static class GeneralTestFeedbackRules
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
        if (len > MaxPossibleFeedbackLength) {
            return Err.ErrFactory.InvalidData(
                $"Accompanying text is too long. Maximum length is {MaxPossibleFeedbackLength}",
                details: $"Maximum length is {MaxPossibleFeedbackLength}. Current length is {len}"
            );
        }
        return ErrOrNothing.Nothing;
    }

    public static ErrOrNothing CheckMaxFeedbackLengthForErrs(ushort value) {
        if (value == 0) {
            return Err.ErrFactory.InvalidData("Maximum Feedback Length must be greater than 0");
        }
        if (value > MaxPossibleFeedbackLength) {
            return Err.ErrFactory.InvalidData(
                $"Maximum Feedback Length cannot be greater than {MaxPossibleFeedbackLength}"
            );
        }
        return ErrOrNothing.Nothing;
    }
}
