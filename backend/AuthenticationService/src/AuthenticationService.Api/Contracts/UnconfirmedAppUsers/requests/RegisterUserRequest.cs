using ApiShared.interfaces;
using AuthenticationService.Domain.Rules;
using SharedKernel.Common.errors;

namespace AuthenticationService.Api.Contracts.UnconfirmedAppUsers.requests;

internal record class RegisterUserRequest(
    string Email,
    string Password,
    string ConfirmPassword
) : IRequestWithValidationNeeded
{
    public RequestValidationResult Validate() {
        if (!Domain.Common.value_objects.Email.IsStringValidEmail(Email)) {
            return Err.ErrFactory.InvalidData(message: "Email is not valid");
        }
        if (PasswordRules.CheckForErr(Password).IsErr(out var err)) {
            return err;
        }
        if (Password != ConfirmPassword) {
            return Err.ErrFactory.InvalidData(message: "Passwords do not match");
        }
        return RequestValidationResult.Success;
    }
}
