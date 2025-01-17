using SharedKernel.Common;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.tests.value_objects;

namespace TestCreationService.Domain.TestAggregate.formats_shared;

public class TestSettings : ValueObject
{

    public AccessLevel TestAccess { get; init; }
    public ResourceAvailabilitySetting AllowRatings { get; init; }
    public ResourceAvailabilitySetting AllowDiscussions { get; init; }
    public bool AllowTestTakenPosts { get; init; }
    public ResourceAvailabilitySetting AllowTagsSuggestions { get; init; }

    public override IEnumerable<object> GetEqualityComponents() =>
        [TestAccess, AllowRatings, AllowDiscussions, AllowTestTakenPosts, AllowTagsSuggestions];
    public TestSettings(
            AccessLevel testAccess,
            ResourceAvailabilitySetting allowRatings,
            ResourceAvailabilitySetting allowDiscussions,
            bool allowTestTakenPosts,
            ResourceAvailabilitySetting allowTagsSuggestions
    ) {
        TestAccess = testAccess;
        AllowRatings = allowRatings;
        AllowDiscussions = allowDiscussions;
        AllowTestTakenPosts = allowTestTakenPosts;
        AllowTagsSuggestions = allowTagsSuggestions;
    }

    public static TestSettings Deafult => new TestSettings(
        AccessLevel.Public,
        allowRatings: ResourceAvailabilitySetting.EnabledPublic,
        allowDiscussions: ResourceAvailabilitySetting.EnabledPublic,
        allowTestTakenPosts: true,
        allowTagsSuggestions: ResourceAvailabilitySetting.EnabledPublic
    );
}
