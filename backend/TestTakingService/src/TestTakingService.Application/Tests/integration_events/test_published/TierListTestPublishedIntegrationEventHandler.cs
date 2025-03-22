using System.Collections.Immutable;
using MediatR;
using SharedKernel.Common.tests.tier_list_format;
using SharedKernel.IntegrationEvents.test_publishing;
using TestTakingService.Application.Common.interfaces.repositories.tests;
using TestTakingService.Domain.TestAggregate.tier_list_format;

namespace TestTakingService.Application.Tests.integration_events.test_published;

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
        TierListFormatTest test = new(
            notification.TestId,
            notification.CreatorId,
            notification.EditorIds.ToImmutableHashSet(),
            notification.InteractionsAccessSettings.TestAccess,
            CreateStyleFromNotification(notification),
            CreateTiersFromNotification(notification),
            shuffleTiers: notification.ShuffleTiers,
            CreateItemsFromNotification(notification),
            shuffleItems: notification.ShuffleItems,
            notification.FeedbackOption
        );
        await _tierListFormatTestsRepository.Add(test);
    }

    private TierListTestItem[] CreateItemsFromNotification(
        TierListTestPublishedIntegrationEvent notification
    ) => notification.Items.Select(r => new TierListTestItem(
        r.Id, name: r.Name, clarification: r.Clarification, r.Content, r.Order
    )).ToArray();

    private TierListTestTier[] CreateTiersFromNotification(
        TierListTestPublishedIntegrationEvent notification
    ) => notification.Tiers.Select(r => new TierListTestTier(
        r.Id,
        name: r.Name,
        description: r.Description,
        r.MaxItemsCountLimit,
        TierListTestTierStyles.CreateNew(backgroundColor: r.Styles.BackgroundColor, textColor: r.Styles.TextColor)
        , r.Order
    )).ToArray();
}