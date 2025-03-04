using DBSeeder.Data.published_tests.published_tests_data;
using Microsoft.EntityFrameworkCore;
using TestManagingService.Infrastructure.Persistence;

namespace DBSeeder.DbSeeders;

public class TestManagingServiceDbSeeder : IDbContextSeeder
{
    private TestManagingDbContext _db;

    public async Task Initialize(string dbConnectionString) {
        DbContextOptions<TestManagingDbContext> options =
            new DbContextOptionsBuilder<TestManagingDbContext>()
                .UseNpgsql(dbConnectionString)
                .Options;

        _db = new TestManagingDbContext(options, new FakePublisher());
    }

    public async Task Commit() {
        await _db.SaveChangesAsync();
    }

    public async Task Rollback() => _db.ChangeTracker.Clear();

    public async Task EnsureExists() {
        await _db.Database.EnsureCreatedAsync();
    }

    public async Task Clear() {
        await _db.Database.EnsureDeletedAsync();
        await _db.Database.EnsureCreatedAsync();
    }

    public async Task ClearAndSeed() {
        await _db.Database.EnsureDeletedAsync();
        await _db.Database.EnsureCreatedAsync();
        await Seed();
    }

    private async Task Seed() {
        try {
            foreach (var test in GeneralFormatPublishedTestsData.AllTests) {
                await _db.BaseTests.AddAsync(test.TestManagingTest);
            }
        }
        catch (Exception e) {
            throw new DbContextSeederException(e, typeof(TestManagingServiceDbSeeder));
        }
    }
}