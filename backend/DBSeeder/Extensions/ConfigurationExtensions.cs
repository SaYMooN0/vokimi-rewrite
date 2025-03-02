using Microsoft.Extensions.Configuration;

namespace DBSeeder.Extensions;

public static class ConfigurationExtensions
{
    public static string GetRequiredConnectionString(this IConfiguration configuration, string name)
    {
        return configuration.GetConnectionString(name) 
               ?? throw new Exception($"{name} connection string is missing in appsettings.json");
    }
}