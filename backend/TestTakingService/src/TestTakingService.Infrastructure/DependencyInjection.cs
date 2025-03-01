using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Common;
using SharedKernel.Common.interfaces;
using SharedKernel.Configs;
using TestTakingService.Application.Common.interfaces.repositories;
using TestTakingService.Application.Common.interfaces.repositories.test_taken_records;
using TestTakingService.Application.Common.interfaces.repositories.tests;
using TestTakingService.Infrastructure.IntegrationEvents.background_service;
using TestTakingService.Infrastructure.IntegrationEvents.integration_events_publisher;
using TestTakingService.Infrastructure.Persistence;
using TestTakingService.Infrastructure.Persistence.repositories;
using TestTakingService.Infrastructure.Persistence.repositories.test_taken_records;
using TestTakingService.Infrastructure.Persistence.repositories.tests;

namespace TestTakingService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        services
            .AddMessageBrokerIntegration(configuration)
            .AddPersistence(configuration)
            .AddMediatR()
            .AddDateTimeService()
            ;

        return services;
    }

    private static IServiceCollection AddMediatR(this IServiceCollection services) {
        services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection)));
        
        return services;
    }

    private static IServiceCollection AddMessageBrokerIntegration(
        this IServiceCollection services,
        IConfiguration configuration
    ) {
        services.Configure<MessageBrokerSettings>(options => configuration.GetSection("MessageBroker").Bind(options));
        services.AddSingleton<IIntegrationEventsPublisher, IntegrationEventsPublisher>();
        services.AddHostedService<ConsumeIntegrationEventsBackgroundService>();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) {
        string dbConnetionString = configuration.GetConnectionString("TestTakingServiceDb")
                                   ?? throw new Exception("Database connection string is not provided.");
        services.AddDbContext<TestTakingDbContext>(options => options.UseNpgsql(dbConnetionString));

        services.AddScoped<IAppUsersRepository, AppUsersRepository>();
        
        services.AddScoped<IBaseTestsRepository, BaseTestsRepository>();
        services.AddScoped<IGeneralFormatTestsRepository, GeneralFormatTestsRepository>();
        
        services.AddScoped<IBaseTestTakenRecordsRepository, BaseTestTakenRecordsRepository>();
        services.AddScoped<IGeneralTestTakenRecordsRepository, GeneralTestTakenRecordsRepository>();

        return services;
    }

    private static IServiceCollection AddDateTimeService(this IServiceCollection services) {
        services.AddSingleton<IDateTimeProvider, UtcDateTimeProvider>();
        return services;
    }
}