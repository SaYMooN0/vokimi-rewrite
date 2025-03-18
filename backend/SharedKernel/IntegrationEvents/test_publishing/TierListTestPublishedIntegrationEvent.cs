using System.Collections.Immutable;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.tier_list_format;
using SharedKernel.Common.tests.tier_list_format.feedback;
using SharedKernel.Common.tests.tier_list_format.items;
using SharedKernel.Common.tests.value_objects;

namespace SharedKernel.IntegrationEvents.test_publishing;

public record TierListTestPublishedIntegrationEvent(
    TestId TestId,
    AppUserId CreatorId,
    ImmutableArray<AppUserId> EditorIds,
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
) : BaseTestPublishedIntegrationEvent(
    TestId, CreatorId, EditorIds, Name, CoverImage, Description, Language,
    PublicationDate, InteractionsAccessSettings, Styles, Tags
);

public record TierListTestPublishedTierDto(
    TierListTestTierId Id,
    ushort Order,
    string Name,
    string? Description,
    ushort? MaxItemsCountLimit,
    TierListTestPublishedTierStylesDto Styles
);

public record TierListTestPublishedItemDto(
    TierListTestItemId Id,
    ushort Order,
    string Name,
    string? Clarification,
    TierListTestItemContentData Content
);

public record TierListTestPublishedTierStylesDto(
    HexColor BackgroundColor,
    HexColor TextColor
);