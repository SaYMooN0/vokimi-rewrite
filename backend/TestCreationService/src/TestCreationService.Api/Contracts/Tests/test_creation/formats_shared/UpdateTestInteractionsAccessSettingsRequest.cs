using ApiShared.interfaces;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects;
using System.Text.Json.Serialization;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Api.Contracts.Tests.test_creation.formats_shared;

internal class UpdateTestInteractionsAccessSettingsRequest : IRequestWithValidationNeeded
{
    public AccessLevel TestAccess { get; init; }
    public bool RatingsEnabled { get; init; }
    public AccessLevel RatingsAccess { get; init; }
    public bool DiscussionsEnabled { get; init; }
    public AccessLevel DiscussionsAccess { get; init; }
    public bool AllowTestTakenPosts { get; init; }
    public bool TagSuggestionsEnabled { get; init; }
    public AccessLevel TagSuggestionsAccess { get; init; }
    public RequestValidationResult Validate() {
        ErrList errs = new();
        errs.AddPossibleErr(TestRules.CheckIfDiscussionsAvailabilityIsCorrect(TestAccess, DiscussionsEnabled, DiscussionsAccess));
        errs.AddPossibleErr(TestRules.CheckIfRatingsAvailabilityIsCorrect(TestAccess, RatingsEnabled, RatingsAccess));
        errs.AddPossibleErr(TestRules.CheckIfTestTakenPostsAvailabilityIsCorrect(TestAccess, AllowTestTakenPosts));
        errs.AddPossibleErr(TestRules.CheckIfTagsSuggestionsAvailabilityIsCorrect(TestAccess, TagSuggestionsEnabled, TagSuggestionsAccess));
        return errs;
    }
    public ResourceAvailabilitySetting RatingsSetting => new(RatingsEnabled, RatingsAccess);
    public ResourceAvailabilitySetting DiscussionsSetting => new(DiscussionsEnabled, DiscussionsAccess);
    public ResourceAvailabilitySetting TagSuggestionsSetting => new(TagSuggestionsEnabled, TagSuggestionsAccess);

}
