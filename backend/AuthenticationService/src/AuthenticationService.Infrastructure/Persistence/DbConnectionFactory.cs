using Npgsql;
using System.Data;

namespace AuthenticationService.Infrastructure.Persistence;

internal class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    public NpgsqlConnectionFactory(string connectionString) {
        _connectionString = connectionString;
    }
    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default) {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}