using ApiShared.interfaces;
using SharedKernel.Common.errors;

namespace AuthenticationService.Api.Contracts.UnconfirmedAppUsers.requests
{
    public record class ConfirmRegistrationRequest(
        string UserId,
        string ConfirmationString
    ) : IRequestWithValidationNeeded
    {
        public RequestValidationResult Validate() {
            if (!Guid.TryParse(UserId, out var _)) {
                return new Err("Incorrect confirmation string");
            }
            if (string.IsNullOrWhiteSpace(ConfirmationString)) {
                return new Err("Incorrect confirmation string");
            }
            return RequestValidationResult.Success;
        }
    }
}
