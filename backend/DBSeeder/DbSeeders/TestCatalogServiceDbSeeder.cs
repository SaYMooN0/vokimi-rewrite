using Microsoft.EntityFrameworkCore;
using TestCatalogService.Infrastructure.Persistence;

namespace DBSeeder.DbSeeders;

public class TestCatalogServiceDbSeeder : IDbContextSeeder
{
    private TestCatalogDbContext _db;

    public async Task Initialize(string dbConnectionString) {
        DbContextOptions<TestCatalogDbContext> options =
            new DbContextOptionsBuilder<TestCatalogDbContext>()
                .UseNpgsql(dbConnectionString)
                .Options;

        _db = new TestCatalogDbContext(options, new FakePublisher());
        await _db.Database.BeginTransactionAsync();
    }

    public async Task Commit() {
        await _db.Database.CommitTransactionAsync();
    }

    public async Task Rollback() {
        await _db.Database.RollbackTransactionAsync();
    }

    public async Task EnsureExists() {
        await _db.Database.EnsureCreatedAsync();
    }

    public async Task Clear() {
        await _db.Database.EnsureDeletedAsync();
        await _db.Database.EnsureCreatedAsync();
    }

    private async Task Seed() {
        try {
            await _db.SaveChangesAsync();
        }
        catch (Exception e) {
            throw new DbContextSeederException(e, typeof(AuthenticationServiceDbSeeder));
        }
    }

    public async Task ClearAndSeed() {
        await _db.Database.EnsureDeletedAsync();
        await _db.Database.EnsureCreatedAsync();
        await Seed();
    }
}