using MediatR;
using SharedKernel.IntegrationEvents.test_publishing;
using TestCreationService.Domain.TestAggregate.general_format.events;
using TestCreationService.Infrastructure.IntegrationEvents.integration_events_publisher;

namespace TestCreationService.Infrastructure.IntegrationEvents;

internal class DomainToIntegrationEventsHandler :
    INotificationHandler<GeneralTestPublishedEvent>
// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventsPublisher _integrationEventsPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventsPublisher integrationEventsPublisher) {
        _integrationEventsPublisher = integrationEventsPublisher;
    }
    public async Task Handle(GeneralTestPublishedEvent notification, CancellationToken cancellationToken) {
        var integrationEvent = new GeneralTestPublishedIntegrationEvent(
            notification.TestId,
            notification.CreatorId,
            notification.EditorIds,
            notification.Name,
            notification.CoverImage,
            notification.Description,
            notification.Language,
            notification.PublicationDate,
            notification.InteractionsAccessSettings,
            notification.Styles,
            notification.Tags,
            notification.TestTakingProcessSettings,
            notification.ShuffleQuestions,
            notification.Questions,
            notification.Results
        );
        await _integrationEventsPublisher.PublishEvent(integrationEvent);
    }
}
