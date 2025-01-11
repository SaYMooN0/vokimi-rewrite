using AuthenticationService.Domain.AppUserAggregate;

namespace AuthenticationService.Application.Common.interfaces;

public interface IJwtTokenService
{
    string GenerateToken(AppUser user);
}
