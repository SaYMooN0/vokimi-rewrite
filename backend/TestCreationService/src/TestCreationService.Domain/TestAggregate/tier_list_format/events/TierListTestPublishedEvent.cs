using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.tier_list_format.feedback;
using SharedKernel.IntegrationEvents.test_publishing;

namespace TestCreationService.Domain.TestAggregate.tier_list_format.events;

public record class TierListTestPublishedEvent(
    TestId TestId,
    AppUserId CreatorId,
    AppUserId[] EditorIds,
    string Name,
    string CoverImage,
    string Description,
    Language Language,
    DateTime PublicationDate,
    TestPublishedInteractionsAccessSettingsDto InteractionsAccessSettings,
    TestPublishedStylesDto Styles,
    string[] Tags,
    TierListTestFeedbackOption FeedbackOption,
    TierListTestPublishedTierDto[] Tiers,
    bool ShuffleTiers,
    TierListTestPublishedItemDto[] Items,
    bool ShuffleItems
) : IDomainEvent;