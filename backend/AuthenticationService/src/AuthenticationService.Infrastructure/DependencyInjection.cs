using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AuthenticationService.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        services
            .AddAuth(configuration)
            .AddMediatR()
            .AddConfigurations(configuration)
            .AddBackgroundServices()
            .AddPersistence();

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration) {
        // todo: add auth

        return services;
    }

    private static IServiceCollection AddMediatR(this IServiceCollection services) {
        services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection)));

        return services;
    }

    private static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration) {
        //services.AddOptions();

        //var messageBrokerSettings = new MessageBrokerSettings();
        //configuration.Bind(MessageBrokerSettings.Section, messageBrokerSettings);

        //services.AddSingleton(Options.Create(messageBrokerSettings));

        return services;
    }

    private static IServiceCollection AddBackgroundServices(this IServiceCollection services) {
        //services.AddSingleton<IIntegrationEventsPublisher, IntegrationEventsPublisher>();
        //services.AddHostedService<PublishIntegrationEventsBackgroundService>();
        //services.AddHostedService<ConsumeIntegrationEventsBackgroundService>();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services) {
        //services.AddDbContext<GymManagementDbContext>(options =>
        //    options.UseSqlite("Data Source = GymManagement.db"));

        //services.AddScoped<IAdminsRepository, AdminsRepository>();
        //services.AddScoped<IGymsRepository, GymsRepository>();
        //services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();

        return services;
    }
}
