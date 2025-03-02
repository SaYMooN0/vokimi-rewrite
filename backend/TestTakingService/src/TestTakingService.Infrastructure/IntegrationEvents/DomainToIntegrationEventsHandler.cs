using MediatR;
using SharedKernel.IntegrationEvents.test_managing.feedback_left;
using TestTakingService.Domain.TestFeedbackRecordAggregate.events;
using TestTakingService.Infrastructure.IntegrationEvents.integration_events_publisher;

namespace TestTakingService.Infrastructure.IntegrationEvents;

internal class DomainToIntegrationEventsHandler
    : INotificationHandler<FeedbackForGeneralTestLeftEvent>
// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventsPublisher _integrationEventsPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventsPublisher integrationEventsPublisher) {
        _integrationEventsPublisher = integrationEventsPublisher;
    }

    public async Task Handle(FeedbackForGeneralTestLeftEvent notification, CancellationToken cancellationToken) {
        FeedbackForGeneralTestLeftIntegrationEvent integrationEvent = new(
            notification.TestId,
            notification.AuthorId,
            notification.CreatedOn,
            notification.Text,
            notification.WasLeftAnonymously
        );
        await _integrationEventsPublisher.PublishEvent(integrationEvent);
    }
}