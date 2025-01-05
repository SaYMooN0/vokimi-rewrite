using AuthenticationService.Domain.AppUserAggregate;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.Common.interfaces;

public interface IAppUsersRepository
{
    Task<Err> AddUser(AppUser appUser);
    Task<Err> AnyUserWithEmail(string email);
    Task<ErrOr<AppUser>> GetUserByEmailAndPasswordHash(string email, string passwordHash);
}
