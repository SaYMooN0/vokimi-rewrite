using AuthenticationService.Application.Common.interfaces;
using AuthenticationService.Application.Common.interfaces.repositories;
using AuthenticationService.Domain.Common;
using AuthenticationService.Infrastructure.Configs;
using AuthenticationService.Infrastructure.Persistence;
using AuthenticationService.Infrastructure.Persistence.dapper_type_handler;
using AuthenticationService.Infrastructure.Persistence.repositories;
using AuthenticationService.Infrastructure.Services;
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

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) {
        string dbConnetionString = configuration.GetConnectionString("AuthServiceDb")
            ?? throw new Exception("Database connection string is not provided.");
        services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(dbConnetionString));
        //services.AddDbContext<GymManagementDbContext>(options =>
        //    options.UseSqlite("Data Source = GymManagement.db"));

        services.AddSingleton<IAppUsersRepository, AppUsersRepository>();
        services.AddSingleton<IUnconfirmedAppUsersRepository, UnconfirmedAppUsersRepository>();
        services.AddSingleton<IPasswordUpdateRequestsRepository, PasswordResetRequestsRepository>();

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        SqlMapper.AddTypeHandler(new EmailTypeHandler());
        SqlMapper.AddTypeHandler(typeof(AppUserId), new GuidEntityIdTypeHandler<AppUserId>());
        SqlMapper.AddTypeHandler(typeof(UnconfirmedAppUserId), new GuidEntityIdTypeHandler<UnconfirmedAppUserId>());


        return services;
    }
    private static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration) {
        //.GetSection("EmailService")
        services.AddTransient<IEmailService, EmailService>();
        return services;
    }
}
