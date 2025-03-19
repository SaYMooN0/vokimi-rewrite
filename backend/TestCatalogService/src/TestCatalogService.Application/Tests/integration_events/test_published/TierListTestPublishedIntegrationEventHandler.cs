using SharedKernel.IntegrationEvents.test_publishing;
using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate.tier_list_format;

namespace TestCatalogService.Application.Tests.integration_events.test_published;

internal class TierListTestPublishedIntegrationEventHandler
    : TestPublishedIntegrationEventHandler<TierListTestPublishedIntegrationEvent>
{
    private readonly ITierListFormatTestsRepository _tierListFormatTestsRepository;

    public TierListTestPublishedIntegrationEventHandler(
        ITierListFormatTestsRepository tierListFormatTestsRepository
    ) {
        _tierListFormatTestsRepository = tierListFormatTestsRepository;
    }

    public override async Task Handle(TierListTestPublishedIntegrationEvent notification,
        CancellationToken cancellationToken) {
        var tiersCount = (ushort)notification.Tiers.Count();
        var itemsCount = (ushort)notification.Items.Count();

        var interactionsAccessSettings = GetInteractionsAccessSettingsFromNotification(notification);
        var tags = GetTagsFromNotification(notification);
        TierListFormatTest newTest = new(
            notification.TestId,
            notification.Name,
            notification.CoverImage,
            notification.Description,
            notification.CreatorId,
            notification.EditorIds,
            notification.PublicationDate,
            notification.Language,
            tags,
            interactionsAccessSettings,
            tiersCount: tiersCount,
            itemsCount: itemsCount
        );
        await _tierListFormatTestsRepository.Add(newTest);
    }
}