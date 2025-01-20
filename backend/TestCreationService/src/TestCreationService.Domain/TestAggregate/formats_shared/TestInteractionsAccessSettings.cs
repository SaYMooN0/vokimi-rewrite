using SharedKernel.Common;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects;
using TestCreationService.Domain.Common;

namespace TestCreationService.Domain.TestAggregate.formats_shared;

public class TestInteractionsAccessSettings : Entity
{
    protected override EntityId EntityId => Id;
    private TestInteractionsAccessSettings() { }

    public TestInteractionsAccessSettingsId Id { get; init; }
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
        ErrList errList = new();

        if (testAccessLevel.IsStricterThan(discussionsSetting.Access)) {
            errList.Add(Err.ErrFactory.InvalidData(
                message: "Discussions access cannot be less restrictive than the test access.",
                details: $"Test Access: {testAccessLevel}, Discussions Access: {discussionsSetting.Access}"
            ));
        }

        if (testAccessLevel.IsStricterThan(ratingsSetting.Access)) {
            errList.Add(Err.ErrFactory.InvalidData(
                message: "Ratings access cannot be less restrictive than the test access.",
                details: $"Test Access: {testAccessLevel}, Ratings Access: {ratingsSetting.Access}"
            ));
        }

        if (allowTestTakenPosts && TestAccess == AccessLevel.Private) {
            errList.Add(Err.ErrFactory.InvalidData(
                message: "TestTakenPosts cannot be enabled when the test is private."
            ));
        }

        if (testAccessLevel.IsStricterThan(tagsSuggestionsSetting.Access)) {
            errList.Add(Err.ErrFactory.InvalidData(
                message: "Tag suggestions access cannot be less restrictive than the test access.",
                details: $"Test Access: {testAccessLevel}, Tags Suggestions Access: {tagsSuggestionsSetting.Access}"
            ));
        }

        if (errList.Any()) {
            return errList;
        }

        TestAccess = testAccessLevel;
        AllowRatings = ratingsSetting;
        AllowDiscussions = discussionsSetting;
        AllowTestTakenPosts = allowTestTakenPosts;
        AllowTagsSuggestions = tagsSuggestionsSetting;

        return ErrListOrNothing.Nothing;
    }
}
