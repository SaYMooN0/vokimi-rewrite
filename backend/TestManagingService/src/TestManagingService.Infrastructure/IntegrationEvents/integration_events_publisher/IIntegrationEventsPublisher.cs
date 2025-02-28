
using SharedKernel.IntegrationEvents;

namespace TestManagingService.Infrastructure.IntegrationEvents.integration_events_publisher;

internal interface IIntegrationEventsPublisher
{
    public Task PublishEvent(IIntegrationEvent integrationEvent);
}
