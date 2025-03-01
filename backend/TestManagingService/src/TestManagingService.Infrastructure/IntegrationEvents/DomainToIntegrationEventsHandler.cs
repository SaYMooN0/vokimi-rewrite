using MediatR;
using SharedKernel.IntegrationEvents;
using SharedKernel.IntegrationEvents.test_managing;
using TestManagingService.Domain.TestAggregate.formats_shared.events;
using TestManagingService.Infrastructure.IntegrationEvents.integration_events_publisher;

namespace TestManagingService.Infrastructure.IntegrationEvents;

internal class DomainToIntegrationEventsHandler :
    INotificationHandler<TagsChangedEvent>
//settings changed
// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventsPublisher _integrationEventsPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventsPublisher integrationEventsPublisher) {
        _integrationEventsPublisher = integrationEventsPublisher;
    }

    public async Task Handle(TagsChangedEvent notification, CancellationToken cancellationToken) {
        var integrationEvent = new PublishedTestTagsChangedIntegrationEvent(
            notification.TestId,
            notification.NewTags.Select(t => t.Value).ToArray()
        );
        await _integrationEventsPublisher.PublishEvent(integrationEvent);
    }
}