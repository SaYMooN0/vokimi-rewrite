using AuthenticationService.Domain.Common.value_objects;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.Common.interfaces;
public interface IEmailService
{
    Task<ErrOrNothing> SendRegistrationConfirmationLink(Email email, string link);
}
