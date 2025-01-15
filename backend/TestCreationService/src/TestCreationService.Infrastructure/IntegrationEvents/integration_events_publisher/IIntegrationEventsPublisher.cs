
using SharedKernel.IntegrationEvents;

namespace TestCreationService.Infrastructure.IntegrationEvents.integration_events_publisher;

internal interface IIntegrationEventsPublisher
{
    public Task PublishEvent(IIntegrationEvent integrationEvent);
}
