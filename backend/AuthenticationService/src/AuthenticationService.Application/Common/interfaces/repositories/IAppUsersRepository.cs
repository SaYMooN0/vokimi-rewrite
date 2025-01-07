using AuthenticationService.Domain.AppUserAggregate;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.Common.interfaces.repositories;

public interface IAppUsersRepository
{
    Task<ErrOrNothing> AddUser(AppUser appUser);
    Task<ErrOr<bool>> AnyUserWithEmail(string email);
    Task<ErrOr<AppUser>> GetUserByEmailAndPasswordHash(string email, string passwordHash);
}
