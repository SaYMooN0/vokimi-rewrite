using AuthenticationService.Domain.AppUserAggregate;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.Common.interfaces.repositories;

public interface IAppUsersRepository
{
    Task AddUser(AppUser appUser);
    Task<bool> AnyUserWithEmail(string email);
    Task<AppUser?> GetUserByEmailAndPasswordHash(string email, string passwordHash);
}
