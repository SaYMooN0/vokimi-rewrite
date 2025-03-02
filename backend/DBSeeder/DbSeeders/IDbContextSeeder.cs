namespace DBSeeder.DbSeeders;

public interface IDbContextSeeder
{
    public Task Initialize(string dbConnectionString);
    public Task Commit();
    public Task Rollback();
    public Task EnsureExists();
    public Task Clear();
    public Task ClearAndSeed();
}