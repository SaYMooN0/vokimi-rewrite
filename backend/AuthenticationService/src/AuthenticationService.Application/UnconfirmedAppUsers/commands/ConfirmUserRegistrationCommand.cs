using MediatR;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.UnconfirmedAppUsers.commands;

public record class ConfirmUserRegistrationCommand : IRequest<ErrOr<string>>;
public class ConfirmUserRegistrationCommandHandler : IRequestHandler<ConfirmUserRegistrationCommand, ErrOr<string>>
{
    public async Task<ErrOr<string>> Handle(ConfirmUserRegistrationCommand request, CancellationToken cancellationToken) {
        //return auth token in case of success
        return Err.ErrFactory.NotImplemented();
    }
}
