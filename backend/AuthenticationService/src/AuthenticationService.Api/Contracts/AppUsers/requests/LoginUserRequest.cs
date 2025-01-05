using ApiShared;
using ApiShared.interfaces;

namespace AuthenticationService.Api.Contracts.AppUsers.requests;

public record class LoginUserRequest() : IRequestWithValidationNeeded
{
    public RequestValidationResult Validate() {
        throw new NotImplementedException();
    }
}
