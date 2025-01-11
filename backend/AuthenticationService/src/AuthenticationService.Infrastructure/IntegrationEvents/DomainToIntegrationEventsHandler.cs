using AuthenticationService.Domain.Events;
using AuthenticationService.Infrastructure.IntegrationEvents.integration_events_publisher;
using MediatR;
using SharedKernel.IntegrationEvents.authentication;

namespace AuthenticationService.Infrastructure.IntegrationEvents;

internal class DomainToIntegrationEventsHandler :
    INotificationHandler<NewAppUserCreated>
// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventsPublisher _integrationEventsPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventsPublisher integrationEventsPublisher) {
        _integrationEventsPublisher = integrationEventsPublisher;
    }

    public async Task Handle(NewAppUserCreated notification, CancellationToken cancellationToken) {
        var integrationEvent = new NewAppUserCreatedIntegrationEvent(notification.CreateUserId);
        await _integrationEventsPublisher.PublishEvent(integrationEvent);
    }
}
