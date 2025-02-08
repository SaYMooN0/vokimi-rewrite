using ApiShared.extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SharedKernel.Configs;

namespace ApiShared.endpoints_filters;

internal class AuthenticationRequiredEndpointFilter : IEndpointFilter
{
    private readonly JwtTokenConfig _jwtConfig;

    public AuthenticationRequiredEndpointFilter(IOptions<JwtTokenConfig> options) {
        _jwtConfig = options.Value;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
        var httpContext = context.HttpContext;
        var userIdOrErr = httpContext.ParseUserIdFromJwtToken(_jwtConfig);

        if (userIdOrErr.IsErr(out var err))
        {
            return CustomResults.Unauthorized(err.WithPrefix("Access denied. Authentication required"));
        }
        httpContext.Items["AppUserId"] = userIdOrErr.GetSuccess();

        return await next(context);
    }
}
