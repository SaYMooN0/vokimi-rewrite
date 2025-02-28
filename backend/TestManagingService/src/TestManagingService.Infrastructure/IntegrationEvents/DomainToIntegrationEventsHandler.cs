using MediatR;
using TestManagingService.Infrastructure.IntegrationEvents.integration_events_publisher;

namespace TestManagingService.Infrastructure.IntegrationEvents;

internal class DomainToIntegrationEventsHandler :
    INotificationHandler<TagsChangedEvent>
// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventsPublisher _integrationEventsPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventsPublisher integrationEventsPublisher) {
        _integrationEventsPublisher = integrationEventsPublisher;
    }

    public async Task Handle(TagsChangedEvent notification, CancellationToken cancellationToken) {
        var integrationEvent = new PublishedTagsChangedEvent(
            notification.TestId,
            
        );
        await _integrationEventsPublisher.PublishEvent(integrationEvent);
    }
}