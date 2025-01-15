
namespace SharedKernel.Configs;

public class JwtTokenConfig
{
    public string SecretKey { get; init; }
    public string Issuer { get; init; }
    public string Audience { get; init; }
}
