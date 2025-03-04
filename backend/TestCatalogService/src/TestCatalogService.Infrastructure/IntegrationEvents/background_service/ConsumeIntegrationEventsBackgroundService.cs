using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedKernel.Configs;
using SharedKernel.IntegrationEvents;

namespace TestCatalogService.Infrastructure.IntegrationEvents.background_service;
internal class ConsumeIntegrationEventsBackgroundService : IHostedService
{
    private readonly ILogger<ConsumeIntegrationEventsBackgroundService> _logger;
    private readonly MessageBrokerSettings _messageBrokerSettings;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private IConnection connection;
    private IChannel channel;
    public ConsumeIntegrationEventsBackgroundService(
        ILogger<ConsumeIntegrationEventsBackgroundService> logger,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<MessageBrokerSettings> messageBrokerOptions
    ) {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _messageBrokerSettings = messageBrokerOptions.Value;
    }
    public async Task StartAsync(CancellationToken cancellationToken) {
        _logger.LogInformation("Starting integration event consumer background service.");

        var connectionFactory = new ConnectionFactory {
            HostName = _messageBrokerSettings.HostName,
            Port = _messageBrokerSettings.Port,
            UserName = _messageBrokerSettings.UserName,
            Password = _messageBrokerSettings.Password
        };
        connection = await connectionFactory.CreateConnectionAsync();
        channel = await connection.CreateChannelAsync();
        await SetupMessageBrokerAsync(cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (sender, eventArgs) => await HandleEventAsync(sender, eventArgs, cancellationToken);
        await channel.BasicConsumeAsync(_messageBrokerSettings.QueueName, autoAck: false, consumer: consumer);
    }
    private async Task SetupMessageBrokerAsync(CancellationToken cancellationToken) {
        await channel.ExchangeDeclareAsync(_messageBrokerSettings.ExchangeName, ExchangeType.Fanout, durable: true);

        var queueDeclareResult = await channel.QueueDeclareAsync(
            queue: _messageBrokerSettings.QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false
        );

        await channel.QueueBindAsync(_messageBrokerSettings.QueueName, _messageBrokerSettings.ExchangeName, routingKey: string.Empty);
    }
    private async Task HandleEventAsync(object sender, BasicDeliverEventArgs eventArgs, CancellationToken cancellationToken) {
        if (cancellationToken.IsCancellationRequested) {
            _logger.LogInformation("Cancellation requested, not consuming integration event.");
            return;
        }

        try {
            _logger.LogInformation("Received integration event. Reading message from queue.");

            using var scope = _serviceScopeFactory.CreateScope();

            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var integrationEvent = JsonSerializer.Deserialize<IIntegrationEvent>(message);
            if (integrationEvent == null) {
                throw new Exception("Integration event is null.");
            }

            _logger.LogInformation(
                "Received integration event of type: {IntegrationEventType}. Publishing event.",
                integrationEvent.GetType().Name);

            var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
            await publisher.Publish(integrationEvent);

            _logger.LogInformation("Integration event published successfully. Sending ack to message broker.");
            await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
        } catch (Exception e) {
            _logger.LogError(e, "Exception occurred while consuming integration event.");
            await channel.BasicNackAsync(eventArgs.DeliveryTag, false, true);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken) {
        try {
            if (channel is not null && connection is not null) {
                await channel.CloseAsync();
                channel.Dispose();

                await connection.CloseAsync();
                connection.Dispose();
            }
        } catch (Exception e) {
            _logger.LogError(e, "Error while stopping the consumer service.");
        }
    }
}
