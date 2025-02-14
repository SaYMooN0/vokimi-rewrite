using SharedKernel.Common.common_enums;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects;

namespace SharedKernel.Common.tests.formats_shared.interaction_access_settings;

public static class TestInteractionsAccessSettingsRules
{
    private static ErrOrNothing CheckFeatureAvailabilityIsCorrect(
        string featureName,
        AccessLevel testAccess,
        ResourceAvailabilitySetting featureSetting
    ) {
        if (featureSetting.IsEnabled && testAccess.IsStricterThan(featureSetting.Access)) {
            return Err.ErrFactory.InvalidData(
                message: $"{featureName} access cannot be less restrictive than the test access",
                details: $"Test Access: {testAccess}, {featureName} Access: {featureSetting.Access}"
            );
        }

        return ErrOrNothing.Nothing;
    }

    public static ErrOrNothing CheckIfCommentsAvailabilityIsCorrect(
        AccessLevel testAccess,
        ResourceAvailabilitySetting commentsSetting
    ) => CheckFeatureAvailabilityIsCorrect("Comments", testAccess, commentsSetting);


    public static ErrOrNothing CheckIfRatingsAvailabilityIsCorrect(
        AccessLevel testAccess,
        ResourceAvailabilitySetting ratingsSetting
    ) => CheckFeatureAvailabilityIsCorrect("Ratings", testAccess, ratingsSetting);

    public static ErrOrNothing CheckIfTagsSuggestionsAvailabilityIsCorrect(
        AccessLevel testAccess,
        ResourceAvailabilitySetting tagsSuggestionSetting
    ) => CheckFeatureAvailabilityIsCorrect("Tags suggestions", testAccess, tagsSuggestionSetting);

    public static ErrOrNothing CheckIfTestTakenPostsAvailabilityIsCorrect(AccessLevel testAccess,
        bool allowTestTakenPosts) {
        if (allowTestTakenPosts && testAccess == AccessLevel.Private) {
            return Err.ErrFactory.InvalidData(message: "TestTakenPosts cannot be enabled when the test is private");
        }

        return ErrOrNothing.Nothing;
    }
}