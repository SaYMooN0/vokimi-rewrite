
namespace AuthenticationService.Infrastructure.Configs;

internal class EmailServiceConfig
{
    public EmailServiceConfig(string host, int port, string user, string password) {
        Host = host;
        Port = port;
        User = user;
        Password = password;
    }

    public string Host { get; init; }
    public int Port { get; init; }
    public string User { get; init; }
    public string Password { get; init; }
    
}
