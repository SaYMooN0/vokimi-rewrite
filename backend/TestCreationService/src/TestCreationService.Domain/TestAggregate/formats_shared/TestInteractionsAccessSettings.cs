using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects;
using TestCreationService.Domain.Common;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Domain.TestAggregate.formats_shared;

public class TestInteractionsAccessSettings : Entity<TestInteractionsAccessSettingsId>
{
    private TestInteractionsAccessSettings() { }
    private TestId TestId { get; init; }
    public AccessLevel TestAccess { get; private set; }
    public ResourceAvailabilitySetting AllowRatings { get; private set; }
    public ResourceAvailabilitySetting AllowDiscussions { get; private set; }
    public bool AllowTestTakenPosts { get; private set; }
    public ResourceAvailabilitySetting AllowTagsSuggestions { get; private set; }

    public static TestInteractionsAccessSettings CreateNew(TestId testId) => new() {
        Id = TestInteractionsAccessSettingsId.CreateNew(),
        TestId = testId,
        TestAccess = AccessLevel.Public,
        AllowRatings = ResourceAvailabilitySetting.EnabledPublic,
        AllowDiscussions = ResourceAvailabilitySetting.EnabledPublic,
        AllowTestTakenPosts = true,
        AllowTagsSuggestions = ResourceAvailabilitySetting.EnabledFollowersOnly
    };
    public ErrListOrNothing Update(
        AccessLevel testAccessLevel,
        ResourceAvailabilitySetting ratingsSetting,
        ResourceAvailabilitySetting discussionsSetting,
        bool allowTestTakenPosts,
        ResourceAvailabilitySetting tagsSuggestionsSetting
    ) {
        ErrList errs = new();
        errs.AddPossibleErr(TestRules.CheckIfRatingsAvailabilityIsCorrect(testAccessLevel, ratingsSetting.IsEnabled, ratingsSetting.Access));
        errs.AddPossibleErr(TestRules.CheckIfDiscussionsAvailabilityIsCorrect(testAccessLevel, discussionsSetting.IsEnabled, discussionsSetting.Access));
        errs.AddPossibleErr(TestRules.CheckIfTestTakenPostsAvailabilityIsCorrect(testAccessLevel, allowTestTakenPosts));
        errs.AddPossibleErr(TestRules.CheckIfTagsSuggestionsAvailabilityIsCorrect(testAccessLevel, tagsSuggestionsSetting.IsEnabled, tagsSuggestionsSetting.Access));
        if (errs.Any()) {
            return errs;
        }

        TestAccess = testAccessLevel;
        AllowRatings = ratingsSetting;
        AllowDiscussions = discussionsSetting;
        AllowTestTakenPosts = allowTestTakenPosts;
        AllowTagsSuggestions = tagsSuggestionsSetting;

        return ErrListOrNothing.Nothing;
    }
}
