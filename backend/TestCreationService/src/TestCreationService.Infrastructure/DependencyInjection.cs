using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Infrastructure.Persistence;
using TestCreationService.Infrastructure.Persistence.repositories;

namespace TestCreationService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        services
            .AddConfigurations(configuration)
            .AddBackgroundServices()
            .AddPersistence(configuration)
            .AddMediatR();

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

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) {
        string dbConnetionString = configuration.GetConnectionString("TestCreationServiceDb")
            ?? throw new Exception("Database connection string is not provided.");
        services.AddDbContext<TestCreationDbContext>(options => options.UseNpgsql(dbConnetionString));

        services.AddScoped<IBaseTestsRepository, BaseTestsRepository>();

        return services;
    }
}
