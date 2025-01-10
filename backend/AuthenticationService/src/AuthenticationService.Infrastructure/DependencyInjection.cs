using AuthenticationService.Application.Common.interfaces;
using AuthenticationService.Application.Common.interfaces.repositories;
using AuthenticationService.Domain.AppUserAggregate;
using AuthenticationService.Domain.Common;
using AuthenticationService.Infrastructure.Configs;
using AuthenticationService.Infrastructure.IntegrationEvents.background_service;
using AuthenticationService.Infrastructure.IntegrationEvents.integration_events_publisher;
using AuthenticationService.Infrastructure.Middleware.eventual_consistency_middleware;
using AuthenticationService.Infrastructure.Persistence;
using AuthenticationService.Infrastructure.Persistence.dapper_type_handler;
using AuthenticationService.Infrastructure.Persistence.repositories;
using AuthenticationService.Infrastructure.Services;
using AuthenticationService.Infrastructure.Services.jwt_service;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Common.EntityIds;

namespace AuthenticationService.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        services
            .AddAuth(configuration)
            .AddConfigurations(configuration)
            .AddBackgroundServices()
            .AddPersistence(configuration)
            .AddEmailService(configuration)
            .AddMediatR()
            ;

        return services;
    }


    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration) {
        services.AddSingleton<IPasswordHasher>(new PasswordHasher());

        services.Configure<JwtTokenServiceConfig>(options => configuration.GetSection("JwtTokenServiceConfig").Bind(options));
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        
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
        services.AddSingleton<IIntegrationEventsPublisher, IntegrationEventsPublisher>();
        services.AddHostedService<PublishIntegrationEventsBackgroundService>();
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
        SqlMapper.AddTypeHandler(new EmailTypeHandler());
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

        SqlMapper.AddTypeHandler(typeof(AppUserId), new GuidEntityIdTypeHandler<AppUserId>());
        SqlMapper.AddTypeHandler(typeof(UnconfirmedAppUserId), new GuidEntityIdTypeHandler<UnconfirmedAppUserId>());



        return services;
    }
    private static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration) {
        services.Configure<EmailServiceConfig>(options => configuration.GetSection("EmailServiceConfig").Bind(options));
        services.AddTransient<IEmailService, EmailService>();
        return services;
    }
}
