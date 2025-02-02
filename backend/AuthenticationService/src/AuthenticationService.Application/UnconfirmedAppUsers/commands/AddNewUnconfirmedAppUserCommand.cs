using AuthenticationService.Application.Common.interfaces;
using AuthenticationService.Application.Common.interfaces.repositories;
using AuthenticationService.Application.Configs;
using AuthenticationService.Domain.UnconfirmedAppUserAggregate;
using MediatR;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;

namespace AuthenticationService.Application.UnconfirmedAppUsers.commands;

public record class AddNewUnconfirmedAppUserCommand(string email, string password) : IRequest<ErrListOrNothing>;


public class AddNewUnconfirmedAppUserCommandHandler : IRequestHandler<AddNewUnconfirmedAppUserCommand, ErrListOrNothing>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUnconfirmedAppUsersRepository _unconfirmedAppUsersRepository;
    private readonly IEmailService _emailService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly FrontendConfig _frontendConfig;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AddNewUnconfirmedAppUserCommandHandler(
        IAppUsersRepository appUsersRepository,
        IUnconfirmedAppUsersRepository unconfirmedAppUsersRepository,
        IEmailService emailService,
        IPasswordHasher passwordHasher,
        FrontendConfig frontendConfig,
        IDateTimeProvider dateTimeProvider
    ) {
        _appUsersRepository = appUsersRepository;
        _unconfirmedAppUsersRepository = unconfirmedAppUsersRepository;
        _emailService = emailService;
        _passwordHasher = passwordHasher;
        _frontendConfig = frontendConfig;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrListOrNothing> Handle(AddNewUnconfirmedAppUserCommand request, CancellationToken cancellationToken) {
        var userToAddCreationRes = UnconfirmedAppUser.Create(
            request.email,
            request.password,
            _passwordHasher.HashPassword,
           _dateTimeProvider
        );
        if (userToAddCreationRes.AnyErr(out var errs)) { return errs; }

        var userToAdd = userToAddCreationRes.GetSuccess();

        bool anyConfirmedWithThisEmail = await _appUsersRepository.AnyUserWithEmail(request.email);
        if (anyConfirmedWithThisEmail) { return new Err(message: "User with this email already exists", source: ErrorSource.Client); }


        UnconfirmedAppUser? existingUnconfirmedUser = await _unconfirmedAppUsersRepository.GetByEmail(request.email);
        if (existingUnconfirmedUser is null) {
            await _unconfirmedAppUsersRepository.AddNew(userToAdd);
        } else {
            await _unconfirmedAppUsersRepository.OverrideExistingWithEmail(userToAdd);
        }

        string link = _frontendConfig.ConfirmRegistrationUrl + $"/{userToAdd.Id}/{userToAdd.ConfirmationString}";

        var sendingErr = await _emailService.SendRegistrationConfirmationLink(userToAdd.Email, link);
        sendingErr.ThrowIfErr();

        return ErrListOrNothing.Nothing;
    }
}
