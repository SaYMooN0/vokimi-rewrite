using AuthenticationService.Application.Common.interfaces.repositories;
using AuthenticationService.Domain.AppUserAggregate;
using AuthenticationService.Infrastructure.Middleware.eventual_consistency_middleware;

namespace AuthenticationService.Infrastructure.Persistence.repositories;

internal class AppUsersRepository : BaseRepository, IAppUsersRepository
{
    public AppUsersRepository(UnitOfWork unitOfWork) : base(unitOfWork) { }

    public async Task AddUser(AppUser appUser) {
        return;
    }

    public async Task<bool> AnyUserWithEmail(string email) {
        return false;
    }

    public async Task<AppUser?> GetUserByEmailAndPasswordHash(string email, string passwordHash) {
        return null;
    }
}
