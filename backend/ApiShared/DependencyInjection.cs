using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Configs;

namespace ApiShared;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthTokenConfig(this IServiceCollection services, IConfiguration configuration) {
        services.Configure<JwtTokenConfig>(options => configuration.GetSection("JwtTokenConfig").Bind(options));
        return services;
    }

}
