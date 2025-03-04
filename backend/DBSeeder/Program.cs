using DBSeeder;
using DBSeeder.DbSeeders;
using DBSeeder.Extensions;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting Database Seeder...");
IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

List<IDbContextSeeder> seeders = [];
// AuthenticationServiceDbSeeder authenticationServiceDbSeeder = await InitAuthServiceDbSeeder();
TestCatalogServiceDbSeeder testCatalogServiceDbSeeder = await InitTestCatalogServiceDbSeeder();
TestTakingServiceDbSeeder testTakingServiceDbSeeder = await InitTestTakingServiceDbSeeder();
TestManagingServiceDbSeeder testManagingServiceDbSeeder = await InitTestManagingServiceDbSeeder();

//perform db actions
try {
    // await authenticationServiceDbSeeder.ClearAndSeed();
    await testCatalogServiceDbSeeder.ClearAndSeed();
    await testTakingServiceDbSeeder.ClearAndSeed();
    await testManagingServiceDbSeeder.ClearAndSeed();
}
catch (DbContextSeederException exception) {
    Console.WriteLine($"No db seeders has been commited. Error during DB seeding: {exception.Message}");
    Console.WriteLine(exception.InnerException);
    Console.WriteLine(exception.StackTrace);
    return;
}

//commit everything
await CommitSeeders();
return;

//------------------------------------------------
async Task<AuthenticationServiceDbSeeder> InitAuthServiceDbSeeder() {
    string connectionString = config.GetRequiredConnectionString("AuthServiceDb");
    AuthenticationServiceDbSeeder seeder = new();
    await seeder.Initialize(connectionString);
    seeders.Add(seeder);
    return seeder;
}

// async Task<TestCreationServiceDbSeeder> InitTestCreationServiceDbSeeder() {
//     string connectionString = config.GetRequiredConnectionString("TestCreationServiceDb");
//     TestCreationServiceDbSeeder seeder = new();
//     await seeder.Initialize(connectionString);
//     seeders.Add(seeder);
//     return seeder;
// }
async Task<TestTakingServiceDbSeeder> InitTestTakingServiceDbSeeder() {
    string connectionString = config.GetRequiredConnectionString("TestTakingServiceDb");
    TestTakingServiceDbSeeder seeder = new();
    await seeder.Initialize(connectionString);
    seeders.Add(seeder);
    return seeder;
}

async Task<TestCatalogServiceDbSeeder> InitTestCatalogServiceDbSeeder() {
    string connectionString = config.GetRequiredConnectionString("TestCatalogServiceDb");
    TestCatalogServiceDbSeeder seeder = new();
    await seeder.Initialize(connectionString);
    seeders.Add(seeder);
    return seeder;
}

async Task<TestManagingServiceDbSeeder> InitTestManagingServiceDbSeeder() {
    string connectionString = config.GetRequiredConnectionString("TestManagingServiceDb");
    TestManagingServiceDbSeeder seeder = new();
    await seeder.Initialize(connectionString);
    seeders.Add(seeder);
    return seeder;
}

async Task CommitSeeders() {
    List<IDbContextSeeder> uncommittedSeeders = [..seeders];

    foreach (var seeder in seeders) {
        try {
            await seeder.Commit();
            uncommittedSeeders.Remove(seeder);
            Console.WriteLine($"-------------|{seeder.GetType().Name} committed");
        }
        catch (Exception exception) {
            Console.WriteLine("Error occurred, rolling back uncommitted seeders...");
            Console.WriteLine(exception);

            await RollbackSpecificSeeders(uncommittedSeeders);
            return;
        }
    }

    Console.WriteLine("All seeders committed successfully.");
}

async Task RollbackSpecificSeeders(List<IDbContextSeeder> seedersToRollback) {
    await Task.WhenAll(seedersToRollback.Select(async seeder => {
        await seeder.Rollback();
        Console.WriteLine($"\t{seeder.GetType().Name} rolled back");
    }));

    Console.WriteLine("Rollback completed.");
}