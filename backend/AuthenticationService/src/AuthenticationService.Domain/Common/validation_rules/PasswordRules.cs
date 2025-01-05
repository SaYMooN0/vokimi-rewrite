using SharedKernel.Common.errors;

namespace AuthenticationService.Domain.Common.validation_rules;

public static class PasswordRules
{
    public const int MinLength = 8;
    public const int MaxLength = 20;

    public static readonly string MinLengthMessage = "Password must be between 8 and 20 characters.";
    public static readonly string MaxLengthMessage = "Password must be between 8 and 20 characters.";
    public static readonly string MustContainLetterMessage = "Password must contain at least one letter.";
    public static readonly string MustContainNumberMessage = "Password must contain at least one number.";

    public static ErrOrNothing CheckForErr(string password) {
        if (password.Length < MinLength || password.Length > MaxLength)
            return Err.ErrFactory.InvalidData(message: MinLengthMessage);

        if (!password.Any(char.IsLetter)) {
            return Err.ErrFactory.InvalidData(message: MustContainLetterMessage);
        }

        if (!password.Any(char.IsDigit)) {
            return Err.ErrFactory.InvalidData(message: MustContainNumberMessage);
        }

        return ErrOrNothing.Nothing;
    }
}
