using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.tests.value_objects;
using System.Collections.Immutable;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.formats_shared.test_styles;

namespace SharedKernel.IntegrationEvents.test_publishing;

public abstract record BaseTestPublishedIntegrationEvent(
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
    string[] Tags
) : IIntegrationEvent;

public record TestPublishedInteractionsAccessSettingsDto(
    AccessLevel TestAccess,
    ResourceAvailabilitySetting AllowRatings,
    ResourceAvailabilitySetting AllowComments,
    bool AllowTestTakenPosts,
    ResourceAvailabilitySetting AllowTagsSuggestions
);

public record TestPublishedStylesDto(
    TestStylesSheetId Id,
    HexColor AccentColor,
    HexColor ErrorsColor,
    TestStylesButtons Buttons
);