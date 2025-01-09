using AuthenticationService.Application.Common.interfaces.repositories;
using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.UnconfirmedAppUserAggregate;
using AuthenticationService.Infrastructure.Middleware.eventual_consistency_middleware;
using Dapper;
using SharedKernel.Common.errors;

namespace AuthenticationService.Infrastructure.Persistence.repositories;

internal class UnconfirmedAppUsersRepository : BaseRepository, IUnconfirmedAppUsersRepository
{

    public UnconfirmedAppUsersRepository(UnitOfWork unitOfWork) : base(unitOfWork) {
    }

    public async Task AddNew(UnconfirmedAppUser unconfirmedAppUser) {
        const string sql = @"
        INSERT INTO unconfirmed_app_users (id, email, password_hash, confirmation_string, creation_time)
        VALUES (@Id, @Email, @PasswordHash, @ConfirmationString, @CreationTime);";

        await ExecuteAsync(sql, unconfirmedAppUser);
    }

    public async Task<UnconfirmedAppUser?> GetByEmail(string email) {
        const string sql = @"
        SELECT id, email, password_hash, confirmation_string, creation_time
        FROM unconfirmed_app_users
        WHERE email = @Email
        LIMIT 1;";

        return await QuerySingleOrDefaultAsync<UnconfirmedAppUser>(sql, new { Email = email });
    }

    public async Task<UnconfirmedAppUser?> GetById(UnconfirmedAppUserId userId) {
        const string sql = @"
        SELECT id, email, password_hash, confirmation_string, creation_time
        FROM unconfirmed_app_users
        WHERE id = @UserId
        LIMIT 1;";

        return await QuerySingleOrDefaultAsync<UnconfirmedAppUser>(sql, new { UserId = userId });
    }

    public async Task OverrideExistingWithEmail(UnconfirmedAppUser unconfirmedAppUser) {
        const string sql = @"
        UPDATE unconfirmed_app_users
        SET password_hash = @PasswordHash,
            confirmation_string = @ConfirmationString,
            creation_time = @CreationTime
        WHERE email = @Email;";

        await ExecuteAsync(sql, unconfirmedAppUser);
    }

    public async Task RemoveById(UnconfirmedAppUserId userId) {
        const string sql = @"
        DELETE FROM unconfirmed_app_users
        WHERE id = @UserId;";

        await ExecuteAsync(sql, new { UserId = userId.Value });
    }
}
