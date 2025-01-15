using AuthenticationService.Application.Common.interfaces.repositories;
using AuthenticationService.Domain.AppUserAggregate;
using AuthenticationService.Infrastructure.Middleware.eventual_consistency_middleware;

namespace AuthenticationService.Infrastructure.Persistence.repositories;

internal class AppUsersRepository : BaseRepository, IAppUsersRepository
{
    public AppUsersRepository(UnitOfWork unitOfWork) : base(unitOfWork) { }

    public async Task Add(AppUser appUser) {
        const string sql = @"
        INSERT INTO app_users (id, email, password_hash, role, registration_date)
        VALUES (@Id, @Email, @PasswordHash, @Role, @RegistrationDate);";

        await ExecuteAsync(sql, appUser);
    }

    public async Task<bool> AnyUserWithEmail(string email) {
        const string sql = @"
        SELECT COUNT(*)
        FROM app_users
        WHERE email = @Email
        LIMIT 1;";

        var count = await QuerySingleOrDefaultAsync<int>(sql, new { Email = email });
        return count > 0;
    }

    public async Task<AppUser?> GetByEmail(string email) {
        const string sql = @"
        SELECT *
        FROM app_users
        WHERE email = @Email
        LIMIT 1;";

        return await QuerySingleOrDefaultAsync<AppUser>(sql, new { Email = email });
    }
}
