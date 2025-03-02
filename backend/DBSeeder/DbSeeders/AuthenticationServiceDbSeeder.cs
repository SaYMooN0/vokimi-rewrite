using System.Data;
using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.Common.value_objects;
using AuthenticationService.Infrastructure.Middleware.eventual_consistency_middleware;
using AuthenticationService.Infrastructure.Persistence;
using AuthenticationService.Infrastructure.Persistence.dapper_type_handler;
using AuthenticationService.Infrastructure.Persistence.repositories;
using Dapper;
using DBSeeder.Data.users.shared;
using SharedKernel.Common.domain.entity;

namespace DBSeeder.DbSeeders;

public class AuthenticationServiceDbSeeder : IDbContextSeeder
{
    private UnitOfWork _unitOfWork;
    private NpgsqlConnectionFactory _dbConnectionFactory;
    private IDbConnection _connection;

    public async Task Initialize(string dbConnectionString) {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        SqlMapper.AddTypeHandler(typeof(Email), new EmailTypeHandler());
        SqlMapper.AddTypeHandler(typeof(DateOnly), new DateOnlyTypeHandler());

        SqlMapper.AddTypeHandler(typeof(AppUserId), new GuidEntityIdTypeHandler<AppUserId>());
        SqlMapper.AddTypeHandler(typeof(UnconfirmedAppUserId), new GuidEntityIdTypeHandler<UnconfirmedAppUserId>());

        _dbConnectionFactory = new(dbConnectionString);
        _connection = await _dbConnectionFactory.CreateConnectionAsync();
        _unitOfWork = new UnitOfWork();
        _unitOfWork.BeginTransaction(_connection);
    }

    private async Task Seed() {
        AppUsersRepository userRepository = new(_unitOfWork);
        try {
            foreach (var user in AppUsersData.AllUsers) {
                await userRepository.Add(user.AuthAppUser);
            }
        }
        catch (Exception e) {
            throw new DbContextSeederException(e, typeof(AuthenticationServiceDbSeeder));
        }
    }

    public Task Commit() {
        _unitOfWork.Commit();
        return Task.CompletedTask;
    }

    public Task Rollback() {
        _unitOfWork.Rollback();
        return Task.CompletedTask;
    }

    public async Task EnsureExists() {
        const string checkTablesQuery = @"
        SELECT EXISTS (
            SELECT FROM information_schema.tables WHERE table_name = 'app_users'
        ) AS app_users_exists,
        EXISTS (
            SELECT FROM information_schema.tables WHERE table_name = 'unconfirmed_app_users'
        ) AS unconfirmed_app_users_exists;";

        var result =
            await _unitOfWork.Connection.QuerySingleAsync<(bool appUsersExists, bool unconfirmedUsersExists)>(
                checkTablesQuery);

        if (!result.appUsersExists || !result.unconfirmedUsersExists) {
            throw new InvalidOperationException("Required tables do not exist in the database.");
        }
    }

    public async Task Clear() {
        const string dropTablesQuery = @"
        DROP TABLE IF EXISTS app_users CASCADE;
        DROP TABLE IF EXISTS unconfirmed_app_users CASCADE;";

        const string createTablesQuery = @"
        CREATE TABLE unconfirmed_app_users (
            Id UUID PRIMARY KEY,
            Password_Hash VARCHAR(64) NOT NULL,
            Email VARCHAR(255) NOT NULL UNIQUE,
            Creation_Time TIMESTAMP NOT NULL,
            Confirmation_String VARCHAR(36) NOT NULL
        );

        CREATE TABLE app_users (
            Id UUID PRIMARY KEY,
            Email VARCHAR(255) NOT NULL UNIQUE,
            Password_Hash VARCHAR(255) NOT NULL,
            Registration_Date DATE NOT NULL,
            Role INT NOT NULL
        );";

        await _unitOfWork.Connection.ExecuteAsync(dropTablesQuery);
        await _unitOfWork.Connection.ExecuteAsync(createTablesQuery);
    }

    public async Task ClearAndSeed() {
        await Clear();
        await Seed();
    }
}