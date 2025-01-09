using AuthenticationService.Application.Common.models;
using AuthenticationService.Domain.AppUserAggregate;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.Common.interfaces;

public interface IJwtTokenService
{
    string GenerateToken(AppUser user);
    ErrOr<JwtTokenUserInfo> GetUserInfoFromToken(string token);
}
