using MediatR;
using SharedKernel.IntegrationEvents.test_publishing;
using TestManagingService.Domain.TestAggregate.formats_shared;

namespace TestManagingService.Application.Tests.integration_events.test_published;

internal abstract class TestPublishedIntegrationEventHandler<T>
    : INotificationHandler<T> where T : BaseTestPublishedIntegrationEvent
{
    public abstract Task Handle(T notification, CancellationToken cancellationToken);

    protected static TestInteractionsAccessSettings GetInteractionsAccessSettingsFromNotification(T notification) =>
        new(
            testAccess: notification.InteractionsAccessSettings.TestAccess,
            allowRatings: notification.InteractionsAccessSettings.AllowRatings,
            allowComments: notification.InteractionsAccessSettings.AllowComments,
            allowTestTakenPosts: notification.InteractionsAccessSettings.AllowTestTakenPosts,
            allowTagsSuggestions: notification.InteractionsAccessSettings.AllowTagSuggestions
        );
}