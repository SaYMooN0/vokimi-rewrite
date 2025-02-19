using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Common.interfaces;
using SharedKernel.Common;
using SharedKernel.Configs;
using TestCatalogService.Domain.Common.interfaces.repositories;
using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Infrastructure.IntegrationEvents.background_service;
using TestCatalogService.Infrastructure.IntegrationEvents.integration_events_publisher;
using TestCatalogService.Infrastructure.Persistence;
using TestCatalogService.Infrastructure.Persistence.repositories;
using TestCatalogService.Infrastructure.Persistence.repositories.tests;

namespace TestCatalogService.Infrastructure;

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

    private static IServiceCollection AddMessageBrokerIntegration(this IServiceCollection services,
        IConfiguration configuration) {
        services.Configure<MessageBrokerSettings>(options => configuration.GetSection("MessageBroker").Bind(options));
        services.AddSingleton<IIntegrationEventsPublisher, IntegrationEventsPublisher>();
        services.AddHostedService<ConsumeIntegrationEventsBackgroundService>();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) {
        string dbConnetionString = configuration.GetConnectionString("TestCatalogServiceDb")
                                   ?? throw new Exception("Database connection string is not provided.");
        services.AddDbContext<TestCatalogDbContext>(options => options.UseNpgsql(dbConnetionString));

        services.AddScoped<IAppUsersRepository, AppUsersRepository>();

        services.AddScoped<IBaseTestsRepository, BaseTestsRepository>();
        services.AddScoped<IGeneralFormatTestsRepository, GeneralFormatTestsRepository>();

        services.AddScoped<ITestTagsRepository, TestTagsRepository>();
        services.AddScoped<ITestCommentsRepository, TestCommentsRepository>();


        return services;
    }

    private static IServiceCollection AddDateTimeService(this IServiceCollection services) {
        services.AddSingleton<IDateTimeProvider, UtcDateTimeProvider>();
        return services;
    }
}