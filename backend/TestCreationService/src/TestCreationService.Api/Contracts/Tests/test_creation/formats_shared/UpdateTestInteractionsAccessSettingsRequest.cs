using ApiShared.interfaces;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects;
using System.Text.Json.Serialization;

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
        if (DiscussionsEnabled && TestAccess.IsStricterThan(DiscussionsAccess)) {
            errs.Add(Err.ErrFactory.InvalidData(
                message: "Discussions access cannot be less restrictive than the test access",
                details: $"Test Access: {TestAccess}, Discussions Access: {DiscussionsAccess}"
            ));
        }

        if (RatingsEnabled && TestAccess.IsStricterThan(RatingsAccess)) {
            errs.Add(Err.ErrFactory.InvalidData(
                message: "Ratings access cannot be less restrictive than the test access",
                details: $"Test Access: {TestAccess}, Ratings Access: {RatingsAccess}"
            ));
        }

        if (AllowTestTakenPosts && TestAccess == AccessLevel.Private) {
            errs.Add(Err.ErrFactory.InvalidData(
                message: "TestTakenPosts cannot be enabled when the test is private"
            ));
        }

        if (TagSuggestionsEnabled && TestAccess.IsStricterThan(TagSuggestionsAccess)) {
            errs.Add(Err.ErrFactory.InvalidData(
                message: "Tag suggestions access cannot be less restrictive than the test access",
                details: $"Test Access: {TestAccess}, Tag Suggestions Access: {TagSuggestionsAccess}"
            ));
        }
        return errs;
    }
    public ResourceAvailabilitySetting RatingsSetting => new(RatingsEnabled, RatingsAccess);
    public ResourceAvailabilitySetting DiscussionsSetting => new(DiscussionsEnabled, DiscussionsAccess);
    public ResourceAvailabilitySetting TagSuggestionsSetting => new(TagSuggestionsEnabled, TagSuggestionsAccess);

}
