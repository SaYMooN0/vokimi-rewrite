using MediatR;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.UnconfirmedAppUsers.commands;

public record class AddNewUnconfirmedAppUserCommand(string email, string password) : IRequest<ErrOrNothing>;

public class AddNewUnconfirmedAppUserCommandHandler : IRequestHandler<AddNewUnconfirmedAppUserCommand, ErrOrNothing>
{
    public async Task<ErrOrNothing> Handle(AddNewUnconfirmedAppUserCommand request, CancellationToken cancellationToken) {
        return Err.ErrFactory.NotImplemented();
    }
}
