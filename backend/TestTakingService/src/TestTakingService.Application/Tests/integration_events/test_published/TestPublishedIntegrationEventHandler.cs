using MediatR;
using SharedKernel.Common.tests.formats_shared.test_styles;
using SharedKernel.IntegrationEvents.test_publishing;

namespace TestTakingService.Application.Tests.integration_events.test_published;

public abstract class TestPublishedIntegrationEventHandler<T> 
    : INotificationHandler<T> where T : BaseTestPublishedIntegrationEvent
{
    public abstract Task Handle(T notification, CancellationToken cancellationToken);
    protected TestStylesSheet CreateStyleFromNotification(T notification) => new(
        notification.Styles.Id,
        notification.TestId,
        notification.Styles.AccentColor,
        notification.Styles.ErrorsColor,
        notification.Styles.Buttons
    );
}