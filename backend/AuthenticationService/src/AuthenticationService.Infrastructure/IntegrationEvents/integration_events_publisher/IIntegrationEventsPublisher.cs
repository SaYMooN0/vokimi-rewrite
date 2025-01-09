
using SharedKernel.IntegrationEvents;

namespace AuthenticationService.Infrastructure.IntegrationEvents.integration_events_publisher;

internal interface IIntegrationEventsPublisher
{
    public void PublishEvent(IIntegrationEvent integrationEvent);

}
