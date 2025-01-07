
using AuthenticationService.Application.Common.interfaces;
using AuthenticationService.Domain.Common.value_objects;
using SharedKernel.Common.errors;

namespace AuthenticationService.Infrastructure;

internal class EmailService : IEmailService
{
    public async Task<ErrOrNothing> SendRegistrationConfirmationLink(Email email, string link) {
        return ErrOrNothing.Nothing;
    }
}
