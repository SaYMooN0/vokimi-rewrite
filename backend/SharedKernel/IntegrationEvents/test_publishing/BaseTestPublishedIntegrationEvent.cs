
using SharedKernel.Common.common_enums;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.tests.test_styles;
using SharedKernel.Common.tests.value_objects;

namespace SharedKernel.IntegrationEvents.test_publishing;

public abstract record BaseTestPublishedIntegrationEvent(
    TestId TestId,
    AppUserId CreatorId,
    AppUserId[] EditorIds,
    string Name, 
    string CoverImage, 
    string Description,
    Language Language,
    TestPublishedInteractionsAccessSettingsDto InteractionsAccessSettings,
    TestPublishedStylesDto Styles, 
    string[] Tags
) : IIntegrationEvent;

public record TestPublishedInteractionsAccessSettingsDto(
    AccessLevel TestAccess,
    ResourceAvailabilitySetting AllowRatings,
    ResourceAvailabilitySetting AllowDiscussions,
    bool AllowTestTakenPosts,
    ResourceAvailabilitySetting AllowTagsSuggestions
);
public record TestPublishedStylesDto(
    HexColor AccentColor,
    HexColor ErrorsColor,
    TestStylesButtons Buttons
);