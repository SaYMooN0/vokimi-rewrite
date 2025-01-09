
namespace AuthenticationService.Infrastructure.Configs;

internal class JwtTokenServiceConfig
{
    public string SecretKey { get; init; }
    public string Issuer { get; init; }
    public string Audience { get; init; }
}
