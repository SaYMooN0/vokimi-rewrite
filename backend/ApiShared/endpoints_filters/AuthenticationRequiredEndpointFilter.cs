using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Configs;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ApiShared.endpoints_filters;

internal class AuthenticationRequiredEndpointFilter : IEndpointFilter
{
    private readonly JwtTokenConfig _jwtConfig;

    public AuthenticationRequiredEndpointFilter(IOptions<JwtTokenConfig> options) {
        _jwtConfig = options.Value;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
        var httpContext = context.HttpContext;
        var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(token)) {
            return CustomResults.Unauthorized(new Err("Access denied. Authentication required"));
        }

        var handler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = _jwtConfig.Issuer,
            ValidAudience = _jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey))
        };

        var principal = handler.ValidateToken(token, tokenValidationParameters, out _);
        var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim)) {
            return CustomResults.Unauthorized(new Err("Access denied. Authentication required"));
        }

        var userId = new AppUserId(Guid.Parse(userIdClaim));
        httpContext.Items["AppUserId"] = userId;

        return await next(context);
    }
}
