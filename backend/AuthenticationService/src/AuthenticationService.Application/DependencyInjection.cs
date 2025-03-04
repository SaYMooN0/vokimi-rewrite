using AuthenticationService.Application.Configs;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
