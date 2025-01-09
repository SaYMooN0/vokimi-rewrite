using AuthenticationService.Infrastructure.IntegrationEvents.settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SharedKernel.IntegrationEvents;


namespace AuthenticationService.Infrastructure.IntegrationEvents.integration_events_publisher;

internal class IntegrationEventsPublisher : IIntegrationEventsPublisher
{
    private readonly MessageBrokerSettings _messageBrokerSettings;
    private readonly IConnection _connection;
    //private readonly IModel _channel;
    public IntegrationEventsPublisher(IOptions<MessageBrokerSettings> messageBrokerOptions) {
        _messageBrokerSettings = messageBrokerOptions.Value;
        IConnectionFactory connectionFactory = new ConnectionFactory {
            HostName = _messageBrokerSettings.HostName,
            Port = _messageBrokerSettings.Port,
            UserName = _messageBrokerSettings.UserName,
            Password = _messageBrokerSettings.Password
        };

        //_connection = connectionFactory.CreateConnection();

        //_channel = _connection.CreateModel();
        //_channel.ExchangeDeclare(_messageBrokerSettings.ExchangeName, ExchangeType.Fanout, durable: true);
    }

    public void PublishEvent(IIntegrationEvent integrationEvent) {
        throw new NotImplementedException();
    }
}
