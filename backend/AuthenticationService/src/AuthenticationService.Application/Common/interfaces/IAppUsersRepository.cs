using AuthenticationService.Domain.AppUserAggregate;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.Common.interfaces;

public interface IAppUsersRepository
{
    Task<ErrOrNothing> AddUser(AppUser appUser);
    Task<bool> AnyUserWithEmail(string email);
    Task<ErrOr<AppUser>> GetUserByEmailAndPasswordHash(string email, string passwordHash);
}
