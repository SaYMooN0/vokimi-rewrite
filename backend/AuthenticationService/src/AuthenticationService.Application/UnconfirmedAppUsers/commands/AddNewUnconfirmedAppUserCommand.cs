using AuthenticationService.Application.Common.interfaces;
using AuthenticationService.Application.Common.interfaces.repositories;
using AuthenticationService.Application.Configs;
using AuthenticationService.Domain.UnconfirmedAppUserAggregate;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.UnconfirmedAppUsers.commands;

public record class AddNewUnconfirmedAppUserCommand(string email, string password) : IRequest<ErrListOrNothing>;


public class AddNewUnconfirmedAppUserCommandHandler : IRequestHandler<AddNewUnconfirmedAppUserCommand, ErrListOrNothing>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUnconfirmedAppUsersRepository _unconfirmedAppUsersRepository;
    private readonly IEmailService _emailService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly FrontendConfig _frontendConfig;

    public AddNewUnconfirmedAppUserCommandHandler(
        IAppUsersRepository appUsersRepository,
        IUnconfirmedAppUsersRepository unconfirmedAppUsersRepository,
        IEmailService emailService,
        IPasswordHasher passwordHasher,
        FrontendConfig frontendConfig
    ) {
        _appUsersRepository = appUsersRepository;
        _unconfirmedAppUsersRepository = unconfirmedAppUsersRepository;
        _emailService = emailService;
        _passwordHasher = passwordHasher;
        _frontendConfig = frontendConfig;
    }

    public async Task<ErrListOrNothing> Handle(AddNewUnconfirmedAppUserCommand request, CancellationToken cancellationToken) {
        var newUserCreation = UnconfirmedAppUser.Create(
            request.email,
            request.password,
            _passwordHasher.HashPassword,
            UtcDateTimeProvider.Instance
        );
        if (newUserCreation.AnyErr(out var errs)) {
            return errs;
        }


        var anyConfirmedWithThisEmail = await _appUsersRepository.AnyUserWithEmail(request.email);
        if (anyConfirmedWithThisEmail.IsErr(out var err)) {
            return err;
        }
        bool confirmedUserExists = anyConfirmedWithThisEmail.GetSuccess();
        if (confirmedUserExists) { return new Err(message: "User with this email already exists", source: ErrorSource.Client); }

        UnconfirmedAppUser unconfirmedAppUser;

        var existingUnconfirmedUser = await _unconfirmedAppUsersRepository.GetByEmail(request.email);
        if (existingUnconfirmedUser.IsErr(out var existingUnconfirmedUserErr)) {
            if (existingUnconfirmedUserErr.Code != Err.ErrCodes.NotFound) {
                return existingUnconfirmedUserErr;
            }

            var addingErr = await _unconfirmedAppUsersRepository.AddNew(newUserCreation.GetSuccess());
            if (addingErr.IsErr(out err)) { return err; }
            
            unconfirmedAppUser = newUserCreation.GetSuccess();
        } else {
            unconfirmedAppUser = existingUnconfirmedUser.GetSuccess();
            
            var overridingErr = await _unconfirmedAppUsersRepository.OverrideUserWithEmail(newUserCreation.GetSuccess());
            if (overridingErr.IsErr(out err)) { return err; }
        }

        string link = _frontendConfig.ConfirmRegistrationUrl + $"/{unconfirmedAppUser.Id}/{unconfirmedAppUser.ConfirmationString}";
        var sendingErr = await _emailService.SendRegistrationConfirmationLink(unconfirmedAppUser.Email, link);
        if (sendingErr.IsErr(out err)) { return err; }

        return ErrListOrNothing.Nothing;
    }
}
