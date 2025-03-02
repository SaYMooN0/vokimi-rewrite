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
AuthenticationServiceDbSeeder authenticationServiceDbSeeder = await InitAuthServiceDbSeeder();

try {
//perform db actions

    await authenticationServiceDbSeeder.ClearAndSeed();
}
catch (DbContextSeederException exception) {
    Console.WriteLine($"No db seeders have been commited. Error during DB seeding: {exception.Message}");
    Console.WriteLine(exception.InnerException);
    Console.WriteLine(exception.StackTrace);
    return;
}

//commit everything
await CommitSeeders();
return;

//------------------------------------------------
async Task<AuthenticationServiceDbSeeder> InitAuthServiceDbSeeder() {
    string authServiceDbConnectionString = config.GetRequiredConnectionString("AuthServiceDb");
    AuthenticationServiceDbSeeder authSeeder = new();
    await authSeeder.Initialize(authServiceDbConnectionString);
    seeders.Add(authSeeder);
    return authSeeder;
}

async Task CommitSeeders() {
    List<IDbContextSeeder> uncommittedSeeders = [..seeders];

    foreach (var seeder in seeders) {
        try {
            await seeder.Commit();
            uncommittedSeeders.Remove(seeder);
            Console.WriteLine($"{seeder.GetType().Name} committed");
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