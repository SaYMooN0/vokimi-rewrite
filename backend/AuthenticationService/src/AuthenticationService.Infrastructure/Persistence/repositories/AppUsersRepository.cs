using AuthenticationService.Application.Common.interfaces;
using AuthenticationService.Application.Common.interfaces.repositories;
using AuthenticationService.Domain.AppUserAggregate;
using Dapper;
using SharedKernel.Common.errors;

namespace AuthenticationService.Infrastructure.Persistence.repositories;

internal class AppUsersRepository : BaseRepository, IAppUsersRepository
{
    public AppUsersRepository(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }

    public Task<ErrOrNothing> AddUser(AppUser appUser) {
        throw new NotImplementedException();
    }
    public async Task<ErrOr<bool>> AnyUserWithEmail(string email) {
        Err err = new(message: "Unable to check if user exists", source: ErrorSource.ThirdParty);
        return await SafeExecute<bool>(err, async (connection) => {
            return false;
            var result = await connection.ExecuteScalarAsync<int>(
                """
                    SELECT COUNT(1)
                    FROM app_users
                    WHERE email = @Email;
                """, new { Email = email });
            return result > 0;
        });
    }
    public Task<ErrOr<AppUser>> GetUserByEmailAndPasswordHash(string email, string passwordHash) {
        throw new NotImplementedException();
    }


}
