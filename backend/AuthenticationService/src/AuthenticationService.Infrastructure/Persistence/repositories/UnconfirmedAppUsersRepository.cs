using AuthenticationService.Application.Common.interfaces;
using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.UnconfirmedAppUserAggregate;
using Dapper;
using SharedKernel.Common.errors;

namespace AuthenticationService.Infrastructure.Persistence.repositories;

internal class UnconfirmedAppUsersRepository : BaseRepository, IUnconfirmedAppUsersRepository
{
    public UnconfirmedAppUsersRepository(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }
    public async Task<ErrOrNothing> AddUnconfirmedAppUser(UnconfirmedAppUser unconfirmedAppUser) {
        Err operationError = new Err("Unable to add new unconfirmed app user", source: ErrorSource.ThirdParty);

        return await SafeExecute(operationError, async (connection) => {
            var result = await connection.ExecuteAsync(
                """
                    insert into unconfirmed_app_users (id, email, password_hash, confirmation_string, creation_time) 
                    values (@Id, @Email, @PasswordHash, @ConfirmationString, @CreationTime);
                """, unconfirmedAppUser);

            if (result > 0) { return ErrOrNothing.Nothing; }
            return operationError;
        });
    }
    public async Task<ErrOr<UnconfirmedAppUser>> GetUnconfirmedAppUserById(UnconfirmedAppUserId userId) {
        Err operationError = new Err("Unable to retrieve unconfirmed app user", source: ErrorSource.ThirdParty);

        return await SafeExecute<UnconfirmedAppUser>(operationError, async (connection) => {
            // select id, email, password_hash, confirmation_code, creation_time
            UnconfirmedAppUser? result = await connection.QuerySingleOrDefaultAsync<UnconfirmedAppUser>(
                """
                    select *
                    from unconfirmed_app_users
                    where id = @UserId
                """, new { UserId = userId });
            if (result is null) {
                return Err.ErrFactory.NotFound(message: "Unable to find unconfirmed user");
            }
            return result;
        });
    }
}
