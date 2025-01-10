using AuthenticationService.Application.Common.interfaces;
using AuthenticationService.Application.Common.interfaces.repositories;
using AuthenticationService.Domain.AppUserAggregate;
using MediatR;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.UnconfirmedAppUsers.commands;

public record class AuthenticateUserCommand(string Email, string Password) : IRequest<ErrOr<string>>;
public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, ErrOr<string>>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthenticateUserCommandHandler(IAppUsersRepository appUsersRepository, IJwtTokenService jwtTokenService, IPasswordHasher passwordHasher) {
        _appUsersRepository = appUsersRepository;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<ErrOr<string>> Handle(AuthenticateUserCommand command, CancellationToken cancellationToken) {
        AppUser? user = await _appUsersRepository.GetByEmail(command.Email);
        if (user is null) {
            return Err.ErrFactory.NotFound("There is no user with this email", source: ErrorSource.Client);
        }
        if (!user.IsPasswordCorrect((passwordHash) => _passwordHasher.VerifyPassword(passwordHash, command.Password))) {
            return Err.ErrFactory.InvalidData("Incorrect password", source: ErrorSource.Client);
        }
        var token = _jwtTokenService.GenerateToken(user);
        return token;
    }
}