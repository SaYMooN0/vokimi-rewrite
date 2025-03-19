using MediatR;
using SharedKernel.IntegrationEvents.test_managing;
using SharedKernel.IntegrationEvents.test_managing.feedback_option_updated;
using SharedKernel.IntegrationEvents.test_managing.tags;
using TestManagingService.Domain.TestAggregate.formats_shared.events;
using TestManagingService.Domain.TestAggregate.general_format.events;
using TestManagingService.Domain.TestAggregate.tier_list_format.events;
using TestManagingService.Infrastructure.IntegrationEvents.integration_events_publisher;

namespace TestManagingService.Infrastructure.IntegrationEvents;

internal class DomainToIntegrationEventsHandler :
    INotificationHandler<TagsChangedEvent>,
    INotificationHandler<TestInteractionsAccessSettingsUpdatedEvent>,
    INotificationHandler<GeneralTestFeedbackOptionUpdatedEvent>,
    INotificationHandler<TierListTestFeedbackOptionUpdatedEvent>
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

    public async Task Handle(
        TestInteractionsAccessSettingsUpdatedEvent notification, CancellationToken cancellationToken
    ) {
        var integrationEvent = new TestInteractionsAccessSettingsUpdatedIntegrationEvent(
            notification.TestId,
            notification.TestAccess,
            AllowRatings: notification.AllowRatings,
            AllowComments: notification.AllowComments,
            AllowTestTakenPosts: notification.AllowTestTakenPosts,
            AllowTagSuggestions: notification.AllowTagSuggestions
        );
        await _integrationEventsPublisher.PublishEvent(integrationEvent);
    }

    public async Task Handle(GeneralTestFeedbackOptionUpdatedEvent notification, CancellationToken cancellationToken) =>
        await _integrationEventsPublisher.PublishEvent(new GeneralTestFeedbackOptionUpdatedIntegrationEvent(
            notification.TestId,
            notification.NewFeedbackOption
        ));
    public async Task Handle(TierListTestFeedbackOptionUpdatedEvent notification, CancellationToken cancellationToken) =>
        await _integrationEventsPublisher.PublishEvent(new TierListTestFeedbackOptionUpdatedIntegrationEvent(
            notification.TestId,
            notification.NewFeedbackOption
        ));

}