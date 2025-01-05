using ApiShared;
using ApiShared.interfaces;

namespace AuthenticationService.Api.Contracts.UnconfirmedAppUsers.requests
{
    public record class ConfirmRegistrationRequest : IRequestWithValidationNeeded
    {
        public RequestValidationResult Validate() {
            throw new NotImplementedException();
        }
    }
}
