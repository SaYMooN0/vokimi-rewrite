using MediatR;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.UnconfirmedAppUsers.commands;

public record class ConfirmUserRegistrationCommand : IRequest<ErrOrNothing>;
public class ConfirmUserRegistrationCommandHandler : IRequestHandler<ConfirmUserRegistrationCommand, ErrOrNothing>
{
    public async Task<ErrOrNothing> Handle(ConfirmUserRegistrationCommand request, CancellationToken cancellationToken) {
        return Err.ErrFactory.NotImplemented();
    }
}
