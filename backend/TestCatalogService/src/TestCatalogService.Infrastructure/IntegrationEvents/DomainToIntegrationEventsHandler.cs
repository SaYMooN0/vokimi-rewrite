using MediatR;
using SharedKernel.IntegrationEvents.test_managing;
using TestCatalogService.Domain.TestAggregate.formats_shared.events;
using TestCatalogService.Infrastructure.IntegrationEvents.integration_events_publisher;

namespace TestCatalogService.Infrastructure.IntegrationEvents;

internal class DomainToIntegrationEventsHandler :
    INotificationHandler<TestCommentReportedEvent>
// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventsPublisher _integrationEventsPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventsPublisher integrationEventsPublisher) {
        _integrationEventsPublisher = integrationEventsPublisher;
    }


    public async Task Handle(TestCommentReportedEvent notification, CancellationToken cancellationToken) {
        TestCommentReportedIntegrationEvent integrationEvent = new(
            notification.TestId,
            notification.ReportAuthorId,
            notification.CommentId,
            notification.Text,
            notification.Reason
        );
        await _integrationEventsPublisher.PublishEvent(integrationEvent);
    }
}