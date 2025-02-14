using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.formats_shared.interaction_access_settings;
using SharedKernel.Common.tests.value_objects;
using TestCreationService.Domain.Common;

namespace TestCreationService.Domain.TestAggregate.formats_shared;

public class TestInteractionsAccessSettings : Entity<TestInteractionsAccessSettingsId>, ITestInteractionsAccessSettings
{
    private TestInteractionsAccessSettings() { }
    public AccessLevel TestAccess { get; private set; }
    public ResourceAvailabilitySetting AllowRatings { get; private set; }
    public ResourceAvailabilitySetting AllowComments { get; private set; }
    public bool AllowTestTakenPosts { get; private set; }
    public ResourceAvailabilitySetting AllowTagsSuggestions { get; private set; }

    public static TestInteractionsAccessSettings CreateNew() => new() {
        Id = TestInteractionsAccessSettingsId.CreateNew(),
        TestAccess = AccessLevel.Public,
        AllowRatings = ResourceAvailabilitySetting.EnabledPublic,
        AllowComments = ResourceAvailabilitySetting.EnabledPublic,
        AllowTestTakenPosts = true,
        AllowTagsSuggestions = ResourceAvailabilitySetting.EnabledFollowersOnly
    };

    public ErrListOrNothing Update(
        AccessLevel testAccessLevel,
        ResourceAvailabilitySetting ratingsSetting,
        ResourceAvailabilitySetting commentsSetting,
        bool allowTestTakenPosts,
        ResourceAvailabilitySetting tagsSuggestionsSetting
    ) {
        ErrList errs = new();
        errs.AddPossibleErr(TestInteractionsAccessSettingsRules.CheckIfRatingsAvailabilityIsCorrect(
            testAccessLevel, ratingsSetting
        ));
        errs.AddPossibleErr(TestInteractionsAccessSettingsRules.CheckIfCommentsAvailabilityIsCorrect(
            testAccessLevel, commentsSetting
        ));
        errs.AddPossibleErr(TestInteractionsAccessSettingsRules.CheckIfTestTakenPostsAvailabilityIsCorrect(
            testAccessLevel, allowTestTakenPosts
        ));
        errs.AddPossibleErr(TestInteractionsAccessSettingsRules.CheckIfTagsSuggestionsAvailabilityIsCorrect(
            testAccessLevel, tagsSuggestionsSetting
        ));
        if (errs.Any()) {
            return errs;
        }

        TestAccess = testAccessLevel;
        AllowRatings = ratingsSetting;
        AllowComments = commentsSetting;
        AllowTestTakenPosts = allowTestTakenPosts;
        AllowTagsSuggestions = tagsSuggestionsSetting;

        return ErrListOrNothing.Nothing;
    }
}