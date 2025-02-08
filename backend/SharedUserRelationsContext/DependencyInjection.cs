using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedUserRelationsContext.repository;

namespace SharedUserRelationsContext;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedUserRelationsContext(
        this IServiceCollection services,
        IConfiguration configuration
    ) {
        var dbConnetionString = configuration.GetConnectionString("UserRelationsDb")
                                ?? throw new Exception("Database connection string is not provided.");
        services.AddDbContext<UserRelationsDbContext>(options => options.UseNpgsql(dbConnetionString));
        services.AddScoped<IUserFollowingsRepository, UserFollowingsRepository>();

        return services;
    }
}