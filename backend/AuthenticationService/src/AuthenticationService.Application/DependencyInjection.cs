using Microsoft.Extensions.DependencyInjection;
using MediatR;
using AuthenticationService.Application.Configs;
using Microsoft.Extensions.Configuration;

namespace AuthenticationService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration) {
        services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection)));

        services.AddConfigurations(configuration);

        return services;
    }
    private static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration) {
        FrontendConfig frotndendConfig = new(
            configuration["FrontendUrl"] ?? throw new Exception("FrontendUrl is not provided"),
            configuration["ConfirmRegistrationUrl"] ?? throw new Exception("ConfirmRegistrationUrl is not provided")
        );
        services.AddSingleton(frotndendConfig);


        return services;
    }
}
