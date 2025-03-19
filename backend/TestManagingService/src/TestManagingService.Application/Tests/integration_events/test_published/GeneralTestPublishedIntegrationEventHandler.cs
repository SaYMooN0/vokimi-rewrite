using SharedKernel.IntegrationEvents.test_publishing;
using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate.general_format;

namespace TestManagingService.Application.Tests.integration_events.test_published;

internal class GeneralTestPublishedIntegrationEventHandler :
    TestPublishedIntegrationEventHandler<GeneralTestPublishedIntegrationEvent>
{
    private IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public GeneralTestPublishedIntegrationEventHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public override async Task Handle(
        GeneralTestPublishedIntegrationEvent notification, CancellationToken cancellationToken
    ) {
        GeneralFormatTest creationRes = new(
            notification.TestId,
            notification.CreatorId,
            notification.EditorIds,
            notification.PublicationDate,
            GetInteractionsAccessSettingsFromNotification(notification),
            notification.FeedbackOption
        );
        await _generalFormatTestsRepository.Add(creationRes);
    }
}