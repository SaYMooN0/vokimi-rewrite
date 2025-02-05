
using SharedKernel.IntegrationEvents;

namespace TestTakingService.Infrastructure.IntegrationEvents.integration_events_publisher;

internal interface IIntegrationEventsPublisher
{
    public Task PublishEvent(IIntegrationEvent integrationEvent);
}
