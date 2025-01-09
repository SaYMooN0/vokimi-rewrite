using AuthenticationService.Application.Common.interfaces;
using AuthenticationService.Application.Common.models;
using AuthenticationService.Domain.AppUserAggregate;
using AuthenticationService.Infrastructure.Configs;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Common.errors;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AuthenticationService.Infrastructure.Services.jwt_service;

internal class JwtTokenService : IJwtTokenService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly TokenValidationParameters _tokenValidationParameters;
    public JwtTokenService(IOptions<JwtTokenServiceConfig> options) {
        _secretKey = options.Value.SecretKey;
        _issuer = options.Value.Issuer;
        _audience = options.Value.Audience;

        _tokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = _issuer,
            ValidAudience = _audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey))
        };
    }

    public string GenerateToken(AppUser user) {
        JwtTokenUserInfo tokenInfo = new JwtTokenUserInfo(user.Id, user.Email);

        var claims = tokenInfo.ToClaims();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _issuer,
            _audience,
            claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ErrOr<JwtTokenUserInfo> GetUserInfoFromToken(string token) {
        if (string.IsNullOrWhiteSpace(token)) {
            return new Err("Authentication token is empty or null");
        }

        try {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out _);
            return JwtTokenUserInfoExtensions.UserInfoFromClaims(principal.Claims);
        } catch (SecurityTokenException) {
            return new Err("Invalid authentication token");
        } catch (Exception ex) {
            //logger new Err($"Token validation error: {ex.Message}");
            return new Err("Something went wrong while processing authentication token");
        }
    }
}
