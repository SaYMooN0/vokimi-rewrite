using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Authentication;

namespace ApiShared;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthTokenConfig(this IServiceCollection services, IConfiguration configuration) {
        services.Configure<JwtTokenConfig>(options => configuration.GetSection("JwtTokenConfig").Bind(options));
        return services;
    }

}
