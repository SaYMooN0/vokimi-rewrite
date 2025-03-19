using System.Collections.Immutable;
using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.IntegrationEvents.test_publishing;
using TestCatalogService.Domain.TestAggregate.formats_shared;

namespace TestCatalogService.Application.Tests.integration_events.test_published;

public abstract class TestPublishedIntegrationEventHandler<T>
    : INotificationHandler<T> where T : BaseTestPublishedIntegrationEvent
{
    public abstract Task Handle(T notification, CancellationToken cancellationToken);

    protected static ImmutableHashSet<TestTagId> GetTagsFromNotification(T notification) =>
        notification.Tags
            .Select(t => {
                var res = TestTagId.Create(t);
                if (res.IsErr(out var err)) {
                    throw new ErrCausedException(err);
                }
                return res.GetSuccess();
            })
            .ToImmutableHashSet();

    protected static TestInteractionsAccessSettings GetInteractionsAccessSettingsFromNotification(T notification) =>
        new(
            testAccess: notification.InteractionsAccessSettings.TestAccess,
            allowRatings: notification.InteractionsAccessSettings.AllowRatings,
            allowComments: notification.InteractionsAccessSettings.AllowComments,
            allowTestTakenPosts: notification.InteractionsAccessSettings.AllowTestTakenPosts,
            allowTagsSuggestions: notification.InteractionsAccessSettings.AllowTagSuggestions
        );
}