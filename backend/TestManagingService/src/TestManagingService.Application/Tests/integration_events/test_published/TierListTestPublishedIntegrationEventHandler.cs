using SharedKernel.IntegrationEvents.test_publishing;
using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate.tier_list_format;

namespace TestManagingService.Application.Tests.integration_events.test_published;

internal class TierListTestPublishedIntegrationEventHandler
    : TestPublishedIntegrationEventHandler<TierListTestPublishedIntegrationEvent>
{
    private readonly ITierListFormatTestsRepository _tierListFormatTestsRepository;

    public TierListTestPublishedIntegrationEventHandler(ITierListFormatTestsRepository tierListFormatTestsRepository) {
        _tierListFormatTestsRepository = tierListFormatTestsRepository;
    }

    public override async Task Handle(
        TierListTestPublishedIntegrationEvent notification, CancellationToken cancellationToken
    ) {
        TierListFormatTest newTest = new(
            notification.TestId,
            notification.CreatorId,
            notification.EditorIds,
            notification.PublicationDate,
            GetInteractionsAccessSettingsFromNotification(notification),
            notification.FeedbackOption
        );
        await _tierListFormatTestsRepository.Add(newTest);
    }
}