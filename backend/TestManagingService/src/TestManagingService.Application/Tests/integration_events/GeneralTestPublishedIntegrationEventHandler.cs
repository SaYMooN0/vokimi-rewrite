using MediatR;
using SharedKernel.IntegrationEvents.test_publishing;
using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate.general_format;

namespace TestManagingService.Application.Tests.integration_events;

internal class GeneralTestPublishedIntegrationEventHandler : INotificationHandler<GeneralTestPublishedIntegrationEvent>
{
    private IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public GeneralTestPublishedIntegrationEventHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task Handle(GeneralTestPublishedIntegrationEvent notification, CancellationToken cancellationToken) {
        ;
        GeneralFormatTest creationRes = GeneralFormatTest.CreateNew(
            notification.TestId,
            notification.CreatorId,
            notification.EditorIds,
            notification.PublicationDate
        );
        await _generalFormatTestsRepository.Add(creationRes);
    }
}