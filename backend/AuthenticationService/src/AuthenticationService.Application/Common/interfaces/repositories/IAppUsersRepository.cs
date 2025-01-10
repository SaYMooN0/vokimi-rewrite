using AuthenticationService.Domain.AppUserAggregate;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.Common.interfaces.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser appUser);
    Task<bool> AnyUserWithEmail(string email);
    Task<AppUser?> GetByEmail(string email);
}
