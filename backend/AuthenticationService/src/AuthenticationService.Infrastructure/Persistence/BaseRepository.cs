using Microsoft.AspNetCore.Connections;
using Npgsql;
using SharedKernel.Common.errors;
using System.Data;

namespace AuthenticationService.Infrastructure.Persistence;

internal abstract class BaseRepository
{
    protected readonly IDbConnectionFactory _connectionFactory;

    protected BaseRepository(IDbConnectionFactory connectionFactory) {
        _connectionFactory = connectionFactory;
    }
    protected async Task<ErrOrNothing> SafeExecute(
        Err errorToReturn,
        Func<IDbConnection, Task<ErrOrNothing>> action
    ) {
        try {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await action(connection);
        } catch (NpgsqlException npgsqlEx) {
            //logger.LogError($"Database error: {npgsqlEx.Message}", npgsqlEx);
        } catch (InvalidOperationException invalidOpEx) {
            //logger.LogError($"Invalid operation: {invalidOpEx.Message}", invalidOpEx);
        } catch (Exception ex) {
            //logger.LogError($"Unexpected error: {ex.Message}", ex);
        }
        return errorToReturn;
    }
    protected async Task<ErrOr<T>> SafeExecute<T>(
        Err errorToReturn,
        Func<IDbConnection, Task<ErrOr<T>>> action
    ) {
        try {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await action(connection);
        } catch (NpgsqlException npgsqlEx) {
            //logger.LogError($"Database error: {npgsqlEx.Message}", npgsqlEx);
        } catch (InvalidOperationException invalidOpEx) {
            //logger.LogError($"Invalid operation: {invalidOpEx.Message}", invalidOpEx);
        } catch (Exception ex) {
            //logger.LogError($"Unexpected error: {ex.Message}", ex);
        }
        return errorToReturn;
    }
}
