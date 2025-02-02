using AuthenticationService.Application.Common.interfaces;
using AuthenticationService.Application.Common.interfaces.repositories;
using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.Common.value_objects;
using AuthenticationService.Infrastructure.Configs;
using AuthenticationService.Infrastructure.IntegrationEvents.background_service;
using AuthenticationService.Infrastructure.IntegrationEvents.integration_events_publisher;
using AuthenticationService.Infrastructure.Middleware.eventual_consistency_middleware;
using AuthenticationService.Infrastructure.Persistence;
using AuthenticationService.Infrastructure.Persistence.dapper_type_handler;
using AuthenticationService.Infrastructure.Persistence.repositories;
using AuthenticationService.Infrastructure.Services;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Common;
using SharedKernel.Common.domain;
using SharedKernel.Common.interfaces;
using SharedKernel.Configs;

namespace AuthenticationService.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        services
            .AddAuthRelatedServices()
            .AddMessageBrokerIntegration(configuration)
            .AddPersistence(configuration)
            .AddEmailService(configuration)
            .AddMediatR()
            .AddDateTimeService()
            ;

        return services;
    }


    private static IServiceCollection AddAuthRelatedServices(this IServiceCollection services) {
        services.AddSingleton<IPasswordHasher>(new PasswordHasher());
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

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
        string dbConnetionString = configuration.GetConnectionString("AuthServiceDb")
            ?? throw new Exception("Database connection string is not provided.");

        services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(dbConnetionString));
        services.AddScoped<UnitOfWork>();

        services.AddScoped<IAppUsersRepository, AppUsersRepository>();
        services.AddScoped<IUnconfirmedAppUsersRepository, UnconfirmedAppUsersRepository>();
        services.AddScoped<IPasswordUpdateRequestsRepository, PasswordResetRequestsRepository>();

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        SqlMapper.AddTypeHandler(typeof(Email), new EmailTypeHandler());
        SqlMapper.AddTypeHandler(typeof(DateOnly), new DateOnlyTypeHandler());

        SqlMapper.AddTypeHandler(typeof(AppUserId), new GuidEntityIdTypeHandler<AppUserId>());
        SqlMapper.AddTypeHandler(typeof(UnconfirmedAppUserId), new GuidEntityIdTypeHandler<UnconfirmedAppUserId>());



        return services;
    }
    private static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration) {
        services.Configure<EmailServiceConfig>(options => configuration.GetSection("EmailServiceConfig").Bind(options));
        services.AddTransient<IEmailService, EmailService>();
        return services;
    }
    private static IServiceCollection AddDateTimeService(this IServiceCollection services) {
        services.AddSingleton<IDateTimeProvider, UtcDateTimeProvider>();
        return services;
    }
}
