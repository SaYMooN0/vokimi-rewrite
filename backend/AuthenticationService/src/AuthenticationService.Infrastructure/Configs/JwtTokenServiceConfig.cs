
namespace AuthenticationService.Infrastructure.Configs;

internal class JwtTokenServiceConfig
{
    public JwtTokenServiceConfig(string secretKey, string issuer, string audience) {
        SecretKey = secretKey;
        Issuer = issuer;
        Audience = audience;
    }

    public string SecretKey { get; init; }
    public string Issuer { get; init; }
    public string Audience { get; init; }
}
