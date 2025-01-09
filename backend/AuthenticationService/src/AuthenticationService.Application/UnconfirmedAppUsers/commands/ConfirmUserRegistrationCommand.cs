using AuthenticationService.Application.Common.interfaces;
using AuthenticationService.Application.Common.interfaces.repositories;
using AuthenticationService.Domain.AppUserAggregate;
using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.UnconfirmedAppUserAggregate;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.UnconfirmedAppUsers.commands;

public record class ConfirmUserRegistrationCommand(UnconfirmedAppUserId unconfirmedUserId, string ConfirmationString) : IRequest<ErrOrNothing>;
public class ConfirmUserRegistrationCommandHandler : IRequestHandler<ConfirmUserRegistrationCommand, ErrOrNothing>
{
    private readonly IUnconfirmedAppUsersRepository _unconfirmedAppUsersRepository;
    public ConfirmUserRegistrationCommandHandler(IUnconfirmedAppUsersRepository unconfirmedAppUsersRepository) {
        _unconfirmedAppUsersRepository = unconfirmedAppUsersRepository;
    }
    public async Task<ErrOrNothing> Handle(ConfirmUserRegistrationCommand request, CancellationToken cancellationToken) {
        UnconfirmedAppUser? unconfirmedAppUser = await _unconfirmedAppUsersRepository.GetById(request.unconfirmedUserId);
        if (unconfirmedAppUser is null) {
            return Err.ErrFactory.NotFound(
                message: "No such unconfirmed user exists",
                details: "Ensure that you used correct link or have not already confirmed this user",
                source: ErrorSource.Client
            );
        }
        var confirmationRes = unconfirmedAppUser.Confirm(request.ConfirmationString);
        if (confirmationRes.IsErr(out var err)) {
            return err;
        }

        return ErrOrNothing.Nothing;
    }
}
