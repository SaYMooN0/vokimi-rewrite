using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Common.interfaces;
using SharedKernel.Common;
using SharedKernel.Configs;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Infrastructure.IntegrationEvents.background_service;
using TestCreationService.Infrastructure.IntegrationEvents.integration_events_publisher;
using TestCreationService.Infrastructure.Persistence;
using TestCreationService.Infrastructure.Persistence.repositories;

namespace TestCreationService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        services
            .AddMessageBrokerIntegration(configuration)
            .AddPersistence(configuration)
            .AddMediatR()
            .AddDateTimeService();

        return services;
    }

    private static IServiceCollection AddMediatR(this IServiceCollection services) {
        services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection)));

        return services;
    }

    private static IServiceCollection AddMessageBrokerIntegration(this IServiceCollection services, IConfiguration configuration) {
        services.Configure<MessageBrokerSettings>(options => configuration.GetSection("MessageBroker").Bind(options));
        services.AddSingleton<IIntegrationEventsPublisher, IntegrationEventsPublisher>();
        services.AddHostedService<ConsumeIntegrationEventsBackgroundService>();

        return services;
    }
    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) {
        string dbConnetionString = configuration.GetConnectionString("TestCreationServiceDb")
            ?? throw new Exception("Database connection string is not provided.");
        services.AddDbContext<TestCreationDbContext>(options => options.UseNpgsql(dbConnetionString));

        services.AddScoped<IBaseTestsRepository, BaseTestsRepository>();
        services.AddScoped<IAppUsersRepository, AppUsersRepository>();
        services.AddScoped<IGeneralFormatTestsRepository, GeneralFormatTestsRepository>();
        services.AddScoped<IScoringFormatTestsRepository, ScoringFormatTestsRepository>();
        services.AddScoped<IGeneralTestQuestionsRepository, GeneralTestQuestionsRepository>();

        return services;
    }
    private static IServiceCollection AddDateTimeService(this IServiceCollection services) {
        services.AddSingleton<IDateTimeProvider, UtcDateTimeProvider>();
        return services;
    }
}
