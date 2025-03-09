using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.domain.value_object;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.formats_shared.interaction_access_settings;
using SharedKernel.Common.tests.value_objects;

namespace TestManagingService.Domain.TestAggregate.formats_shared;

public class TestInteractionsAccessSettings : ValueObject, ITestInteractionsAccessSettings
{
    private TestInteractionsAccessSettings() { }
    public AccessLevel TestAccess { get; private set; }
    public ResourceAvailabilitySetting AllowRatings { get; private set; }
    public ResourceAvailabilitySetting AllowComments { get; private set; }
    public bool AllowTestTakenPosts { get; private set; }
    public bool AllowTagsSuggestions { get; private set; }

    public TestInteractionsAccessSettings(
        AccessLevel testAccess,
        ResourceAvailabilitySetting allowRatings,
        ResourceAvailabilitySetting allowComments,
        bool allowTestTakenPosts,
        bool allowTagsSuggestions
    ) {
        TestAccess = testAccess;
        AllowRatings = allowRatings;
        AllowComments = allowComments;
        AllowTestTakenPosts = allowTestTakenPosts;
        AllowTagsSuggestions = allowTagsSuggestions;
    }

    public override IEnumerable<object> GetEqualityComponents() {
        yield return TestAccess;
        yield return AllowRatings;
        yield return AllowComments;
        yield return AllowTestTakenPosts;
        yield return AllowTagsSuggestions;
    }

    public delegate Task<bool> CheckUserFollowsCreatorAsyncDelegate(AppUserId userId);

    public delegate bool IsUserCreatorOrEditorDelegate(AppUserId userId);

    public async Task<ErrOrNothing> CheckUserAccessToComment(
        AppUserId userId,
        IsUserCreatorOrEditorDelegate isUserCreatorOrEditor,
        CheckUserFollowsCreatorAsyncDelegate checkUserFollowsCreatorAsync
    ) => !AllowComments.IsEnabled
        ? Err.ErrFactory.NoAccess("Commenting is disabled for this test.")
        : AllowComments.Access switch {
            AccessLevel.Public => ErrOrNothing.Nothing,
            AccessLevel.Private =>
                CheckAccessForPrivateFields(userId,
                    "Test comments are private. You must be the creator or editor to comment.", isUserCreatorOrEditor),
            AccessLevel.FollowersOnly => await CheckAccessForFollowingRequiredFields(userId,
                "You must be following the test creator to comment.", checkUserFollowsCreatorAsync),
            _ => Err.ErrFactory.NoAccess("This test has unknown comment access level.")
        };

    public async Task<ErrOrNothing> CheckUserAccessToRate(
        AppUserId userId,
        IsUserCreatorOrEditorDelegate isUserCreatorOrEditor,
        CheckUserFollowsCreatorAsyncDelegate checkUserFollowsCreatorAsync
    ) => !AllowRatings.IsEnabled
        ? Err.ErrFactory.NoAccess("Rating is disabled for this test.")
        : AllowRatings.Access switch {
            AccessLevel.Public => ErrOrNothing.Nothing,
            AccessLevel.Private =>
                CheckAccessForPrivateFields(userId,
                    "Test ratings are private. You must be the creator or editor to rate.", isUserCreatorOrEditor),
            AccessLevel.FollowersOnly => await CheckAccessForFollowingRequiredFields(userId,
                "You must be following the test creator to rate this test.", checkUserFollowsCreatorAsync),
            _ => Err.ErrFactory.NoAccess("This test has unknown rating access level.")
        };

    public async Task<ErrOrNothing> CheckUserAccessToViewTest(
        AppUserId userId,
        IsUserCreatorOrEditorDelegate isUserCreatorOrEditor,
        CheckUserFollowsCreatorAsyncDelegate checkUserFollowsCreatorAsync
    ) => TestAccess switch {
        AccessLevel.Public => ErrOrNothing.Nothing,
        AccessLevel.Private =>
            CheckAccessForPrivateFields(userId,
                "Test is private. You must be the creator or editor to view this test.", isUserCreatorOrEditor),
        AccessLevel.FollowersOnly => await CheckAccessForFollowingRequiredFields(userId,
            "You must be following the test creator to view this test.", checkUserFollowsCreatorAsync),
        _ => Err.ErrFactory.NoAccess("This test has unknown access level.")
    };


    private async Task<ErrOrNothing> CheckAccessForFollowingRequiredFields(
        AppUserId userId,
        string errMessage,
        CheckUserFollowsCreatorAsyncDelegate checkUserFollowsCreatorAsync
    ) => (await checkUserFollowsCreatorAsync(userId)) ? ErrOrNothing.Nothing : Err.ErrFactory.NoAccess(errMessage);

    private ErrOrNothing CheckAccessForPrivateFields(
        AppUserId userId,
        string errMessage,
        IsUserCreatorOrEditorDelegate checkUserCreatorOrEditor
    ) => checkUserCreatorOrEditor(userId) ? ErrOrNothing.Nothing : Err.ErrFactory.NoAccess(errMessage);

    public ErrListOrNothing Update(
        AccessLevel testAccessLevel,
        ResourceAvailabilitySetting ratingsSetting,
        ResourceAvailabilitySetting commentsSetting,
        bool allowTestTakenPosts,
        bool tagsSuggestionsSetting
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