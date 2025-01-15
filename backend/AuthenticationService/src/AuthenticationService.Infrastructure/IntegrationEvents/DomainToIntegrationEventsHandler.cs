using AuthenticationService.Domain.AppUserAggregate.events;
using AuthenticationService.Infrastructure.IntegrationEvents.integration_events_publisher;
using MediatR;
using SharedKernel.IntegrationEvents.authentication;

namespace AuthenticationService.Infrastructure.IntegrationEvents;

internal class DomainToIntegrationEventsHandler :
    INotificationHandler<NewAppUserCreatedEvent>
// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventsPublisher _integrationEventsPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventsPublisher integrationEventsPublisher) {
        _integrationEventsPublisher = integrationEventsPublisher;
    }

    public async Task Handle(NewAppUserCreatedEvent notification, CancellationToken cancellationToken) {
        var integrationEvent = new NewAppUserCreatedIntegrationEvent(notification.CreatedUserId);
        await _integrationEventsPublisher.PublishEvent(integrationEvent);
    }
}
