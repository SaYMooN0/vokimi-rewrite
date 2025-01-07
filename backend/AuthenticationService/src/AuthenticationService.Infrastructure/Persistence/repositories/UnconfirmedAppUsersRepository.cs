using AuthenticationService.Application.Common.interfaces.repositories;
using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.UnconfirmedAppUserAggregate;
using Dapper;
using SharedKernel.Common.errors;

namespace AuthenticationService.Infrastructure.Persistence.repositories;

internal class UnconfirmedAppUsersRepository : BaseRepository, IUnconfirmedAppUsersRepository
{
    public UnconfirmedAppUsersRepository(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }

    public async Task<ErrOr<UnconfirmedAppUser>> GetByEmail(string email) {
        Err err = new(message: "Unable to retrieve unconfirmed user by email", source: ErrorSource.ThirdParty);

        return await SafeExecute<UnconfirmedAppUser>(err, async (connection) => {
            var result = await connection.QuerySingleOrDefaultAsync<UnconfirmedAppUser>(
                """
                    SELECT id, email, password_hash, confirmation_string, creation_time
                    FROM unconfirmed_app_users
                    WHERE email = @Email
                    LIMIT 1;
                """, new { Email = email });

            if (result is null) {
                return Err.ErrFactory.NotFound(message: "Unconfirmed user with this email not found", source: ErrorSource.ThirdParty);
            }

            return result;
        });
    }
    public async Task<ErrOrNothing> AddNew(UnconfirmedAppUser unconfirmedAppUser) {
        Err err = new Err("Unable to add new unconfirmed app user", source: ErrorSource.ThirdParty);

        return await SafeExecute(err, async (connection) => {
            var result = await connection.ExecuteAsync(
                """
                    INSERT INTO unconfirmed_app_users (id, email, password_hash, confirmation_string, creation_time)
                    VALUES (@Id, @Email, @PasswordHash, @ConfirmationString, @CreationTime);
                """, unconfirmedAppUser);

            if (result > 0) {
                return ErrOrNothing.Nothing;
            }

            return err;
        });
    }

    public async Task<ErrOrNothing> OverrideUserWithEmail(UnconfirmedAppUser unconfirmedAppUser) {
        Err err = new Err("Unable to override unconfirmed app user", source: ErrorSource.ThirdParty);

        return await SafeExecute(err, async (connection) => {
            var result = await connection.ExecuteAsync(
                """
                    UPDATE unconfirmed_app_users
                    SET password_hash = @PasswordHash,
                    confirmation_string = @ConfirmationString,
                    creation_time = @CreationTime
                    WHERE email = @Email;
                """, unconfirmedAppUser);

            if (result > 0) {
                return ErrOrNothing.Nothing;
            }

            return err;
        });
    }

    public async Task<ErrOr<UnconfirmedAppUser>> GetById(UnconfirmedAppUserId userId) {
        Err operationError = new Err("Unable to retrieve unconfirmed app user", source: ErrorSource.ThirdParty);

        return await SafeExecute<UnconfirmedAppUser>(operationError, async (connection) => {
            UnconfirmedAppUser? result = await connection.QuerySingleOrDefaultAsync<UnconfirmedAppUser>(
                """
                    SELECT *
                    FROM unconfirmed_app_users
                    WHERE id = @UserId
                    LIMIT 1;
                """, new { UserId = userId });
            if (result is null) {
                return Err.ErrFactory.NotFound(message: "Unable to find unconfirmed user");
            }
            return result;
        });
    }


}
