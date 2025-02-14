using ApiShared.interfaces;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects;
using SharedKernel.Common.tests.formats_shared.interaction_access_settings;

namespace TestCreationService.Api.Contracts.Tests.test_creation.formats_shared;

internal class UpdateTestInteractionsAccessSettingsRequest : IRequestWithValidationNeeded
{
    public AccessLevel TestAccess { get; init; }
    public bool RatingsEnabled { get; init; }
    public AccessLevel RatingsAccess { get; init; }
    public bool CommentsEnabled { get; init; }
    public AccessLevel CommentsAccess { get; init; }
    public bool AllowTestTakenPosts { get; init; }
    public bool TagSuggestionsEnabled { get; init; }
    public AccessLevel TagSuggestionsAccess { get; init; }

    public RequestValidationResult Validate() {
        ErrList errs = new();
        errs.AddPossibleErr(TestInteractionsAccessSettingsRules.CheckIfCommentsAvailabilityIsCorrect(
            TestAccess, CommentsSetting
        ));
        errs.AddPossibleErr(TestInteractionsAccessSettingsRules.CheckIfRatingsAvailabilityIsCorrect(
            TestAccess, RatingsSetting
        ));
        errs.AddPossibleErr(TestInteractionsAccessSettingsRules.CheckIfTestTakenPostsAvailabilityIsCorrect(
            TestAccess, AllowTestTakenPosts
        ));
        errs.AddPossibleErr(TestInteractionsAccessSettingsRules.CheckIfTagsSuggestionsAvailabilityIsCorrect(
            TestAccess, TagSuggestionsSetting
        ));
        return errs;
    }

    public ResourceAvailabilitySetting RatingsSetting => new(RatingsEnabled, RatingsAccess);
    public ResourceAvailabilitySetting CommentsSetting => new(CommentsEnabled, CommentsAccess);
    public ResourceAvailabilitySetting TagSuggestionsSetting => new(TagSuggestionsEnabled, TagSuggestionsAccess);
}