using ApiShared.interfaces;
using Microsoft.AspNetCore.Http;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using SharedKernel.Common.domain.entity;
using SharedKernel.Configs;

namespace ApiShared.extensions;

public static class HttpContextExtensions
{
    public static T GetValidatedRequest<T>(this HttpContext context) where T : class, IRequestWithValidationNeeded {
        if (!context.Items.TryGetValue("ValidatedRequest", out var validatedRequest)) {
            throw new InvalidDataException(
                "Trying to access validated request on the request that has not passed the validation"
            );
        }

        if (validatedRequest is T request) {
            return request;
        }

        throw new InvalidCastException("Request type mismatch");
    }

    public static AppUserId GetAuthenticatedUserId(this HttpContext context) {
        if (!context.Items.TryGetValue("appUserId", out var userIdObj) || userIdObj is not AppUserId userId) {
            throw new UnauthorizedAccessException("User is not authenticated or AppUserId is missing");
        }

        return userId;
    }

    public static TestId GetTestIdFromRoute(this HttpContext context) {
        var testIdString = context.Request.RouteValues["testId"]?.ToString() ?? "";
        if (!Guid.TryParse(testIdString, out var testGuid)) {
            throw new ErrCausedException(Err.ErrFactory.InvalidData(
                "Invalid test id",
                "Couldn't parse test id from route"
            ));
        }

        return new TestId(testGuid);
    }

    public static ErrOr<AppUserId> ParseUserIdFromJwtToken(this HttpContext httpContext, JwtTokenConfig jwtConfig) {
        var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (string.IsNullOrEmpty(token)) {
            return Err.ErrFactory.Unauthorized(
                "User is not authenticated",
                details: "Log in to your account"
            );
        }

        var handler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidAudience = jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey))
        };

        try {
            var principal = handler.ValidateToken(token, tokenValidationParameters, out _);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim)) {
                return Err.ErrFactory.Unauthorized(
                    "User is not authenticated",
                    details: "Log in to your account"
                );
            }

            return new AppUserId(Guid.Parse(userIdClaim));
        }
        catch (Exception) {
            return Err.ErrFactory.Unauthorized(
                "User is not authenticated",
                details: "Log in to your account"
            );
        }
    }
}