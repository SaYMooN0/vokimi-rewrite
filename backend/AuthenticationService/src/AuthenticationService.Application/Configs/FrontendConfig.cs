namespace AuthenticationService.Application.Configs;

public class FrontendConfig
{
    public string Url { get; init; }
    public string ConfirmRegistrationUrl { get; init; }

    public FrontendConfig(string url, string confirmRegistrationUrl) {
        Url = url;
        ConfirmRegistrationUrl = confirmRegistrationUrl;
    }
}
