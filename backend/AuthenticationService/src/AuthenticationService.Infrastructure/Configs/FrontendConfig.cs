namespace AuthenticationService.Infrastructure.Configs;

public class FrontendConfig
{
    public string Url { get; init; }

    public FrontendConfig(string url) {
        Url = url;
    }
}
