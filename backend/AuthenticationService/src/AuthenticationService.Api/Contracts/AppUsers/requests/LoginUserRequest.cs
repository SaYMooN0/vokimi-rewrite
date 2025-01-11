using ApiShared.interfaces;
using SharedKernel.Common.errors;

namespace AuthenticationService.Api.Contracts.AppUsers.requests;

public record class LoginUserRequest(string Email, string Password) : IRequestWithValidationNeeded
{
    public RequestValidationResult Validate() {
        if (!Domain.Common.value_objects.Email.IsStringValidEmail(Email)) {
            return Err.ErrFactory.InvalidData(message: "Email is not valid");
        }
        if (string.IsNullOrWhiteSpace(Password)) {
            return Err.ErrFactory.InvalidData(message: "Password cannot be empty");

        }
        return RequestValidationResult.Success;
    }
}
