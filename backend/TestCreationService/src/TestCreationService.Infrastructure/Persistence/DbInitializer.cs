namespace TestCreationService.Infrastructure.Persistence;

internal class DbInitializer
{
    public static async Task InitializeDb(TestCreationDbContext dbContext) {

        //await RecreateDb(dbContext);
        await dbContext.Database.EnsureCreatedAsync();
    }
    private static async Task RecreateDb(TestCreationDbContext db) {
        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
    }
}
