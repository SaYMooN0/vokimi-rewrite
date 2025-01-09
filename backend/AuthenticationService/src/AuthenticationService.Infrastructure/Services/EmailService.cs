using AuthenticationService.Application.Common.interfaces;
using AuthenticationService.Domain.Common.value_objects;
using AuthenticationService.Infrastructure.Configs;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using SharedKernel.Common.errors;

namespace AuthenticationService.Infrastructure.Services;

internal class EmailService : IEmailService
{
    private readonly string _host;
    private readonly int _port;
    private readonly string _username;
    private readonly string _password;
    public EmailService(IOptions<EmailServiceConfig> options) {
        _host = options.Value.Host;
        _port = options.Value.Port;
        _username = options.Value.Username;
        _password = options.Value.Password;
    }
    public async Task<ErrOrNothing> SendRegistrationConfirmationLink(Email email, string link) {
        string subject = "Please confirm your email";
        string body =
            $"""
                <p>
                    Thank you for registering. Please click the link below to confirm your email:
                </p>
                <p>
                    <a href='{link}'>Confirm Email</a>
                </p>
            """;
        return await SendEmailWithHtmlBody(to: email, subject, body);
    }
    private async Task<ErrOrNothing> SendEmailWithHtmlBody(
           Email to,
           string subject,
           string body
       ) {
        try {
            MimeMessage message = new();
            message.From.Add(new MailboxAddress("Vokimi", _username));
            message.To.Add(new MailboxAddress("", to.ToString()));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var client = await ConfigureSmtpClient();
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return ErrOrNothing.Nothing;
        } catch {
            return new Err("Could not establish connection to send an email");
        }
    }
    private async Task<SmtpClient> ConfigureSmtpClient() {
        var client = new SmtpClient();
        await client.ConnectAsync(_host, _port, true);
        await client.AuthenticateAsync(_username, _password);
        return client;
    }
}
