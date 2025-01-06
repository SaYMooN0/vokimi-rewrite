using AuthenticationService.Application.Common.interfaces;
using AuthenticationService.Domain.AppUserAggregate;
using SharedKernel.Common.errors;

namespace AuthenticationService.Infrastructure.Persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    public Task<ErrOrNothing> AddUser(AppUser appUser) {
        throw new NotImplementedException();
    }

    public Task<bool> AnyUserWithEmail(string email) {
        throw new NotImplementedException();
    }

    public Task<ErrOr<AppUser>> GetUserByEmailAndPasswordHash(string email, string passwordHash) {
        throw new NotImplementedException();
    }
}
