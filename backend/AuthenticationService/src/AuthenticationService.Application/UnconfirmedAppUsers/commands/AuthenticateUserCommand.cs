using MediatR;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.UnconfirmedAppUsers.commands;

public record class AuthenticateUserCommand(string Email, string Password) : IRequest<ErrOr<string>>;
public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, ErrOr<string>>
{
    //private readonly IUserRepository _userRepository;
    //private readonly IJwtTokenService _jwtTokenService;

    //public AuthenticateUserCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService) {
    //    _userRepository = userRepository;
    //    _jwtTokenService = jwtTokenService;
    //}

    public async Task<ErrOr<string>> Handle(AuthenticateUserCommand command, CancellationToken cancellationToken) {
        return Err.ErrFactory.NotImplemented();
        //var user = await _userRepository.GetByEmailAsync(command.Email);
        //if (user == null || !user.ValidatePassword(command.Password)) {
        //    return ErrOr<string>.Err("Invalid credentials");
        //}

        //var token = _jwtTokenService.GenerateToken(user);
        //return token;
    }
}