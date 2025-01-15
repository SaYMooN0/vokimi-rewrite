using SharedKernel.IntegrationEvents.authentication;
using TestCreationService.Infrastructure.IntegrationEvents.integration_events_publisher;

namespace TestCreationService.Infrastructure.IntegrationEvents;

internal class DomainToIntegrationEventsHandler 
// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventsPublisher _integrationEventsPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventsPublisher integrationEventsPublisher) {
        _integrationEventsPublisher = integrationEventsPublisher;
    }
}
