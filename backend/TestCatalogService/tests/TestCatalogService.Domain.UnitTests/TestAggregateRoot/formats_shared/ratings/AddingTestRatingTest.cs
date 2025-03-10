using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects;
using TestCatalogService.Domain.AppUserAggregate.events;
using TestCatalogService.Domain.TestAggregate;
using TestCatalogService.Domain.TestAggregate.formats_shared;
using TestCatalogService.Domain.UnitTests.TestAggregateRoot.test_consts;

namespace TestCatalogService.Domain.UnitTests.TestAggregateRoot.formats_shared.ratings;

public class AddingTestRatingTest
{
    private readonly AppUserId _testUserId = new(Guid.NewGuid());

    private async Task<BaseTest> CreateTestWithRatings(
        Dictionary<AppUserId, ushort>? initialRatings = null,
        TestInteractionsAccessSettings? customInteractionsAccessSettings = null
    ) {
        var test = TestsSharedTestsConsts.CreateBaseTest(customInteractionsAccessSettings);

        if (initialRatings != null) {
            foreach (var (userId, rating) in initialRatings) {
                var ratingRes = await test.AddRating(
                    userId,
                    rating,
                    TestsSharedConsts.DateTimeProviderInstance
                );
                Assert.False(ratingRes.IsErr(), "Failed to initialize test with ratings.");
            }
        }

        return test;
    }

    [Fact]
    public Task AddRating_WhenRatingsAreDisabled_ShouldReturnError() {
        // Arrange
        var test = CreateTestWithRatings(customInteractionsAccessSettings: new TestInteractionsAccessSettings(
                AccessLevel.Public,
                ResourceAvailabilitySetting.Disabled,
                ResourceAvailabilitySetting.EnabledPublic,
                allowTagsSuggestions: false,
                allowTestTakenPosts: true
            )
        );
        // Act
        var result = await test.AddRating(_testUserId, 5, TestsSharedConsts.DateTimeProviderInstance);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Contains("This user has already rated this test", err.Message);
    }

    [Fact]
    public async Task AddRating_WhenUserHasAlreadyRated_ShouldReturnError() {
        // Arrange
        var initialRatings = new Dictionary<AppUserId, ushort> { { _testUserId, 4 } };
        var test = await CreateTestWithRatings(initialRatings);

        // Act
        var result = test.AddRating(_testUserId, 5, TestsSharedConsts.DateTimeProviderInstance);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Contains("This user has already rated this test", err.Message);
    }

    [Fact]
    public async Task AddRating_ShouldReturnError_WhenRatingIsOutOfRange() {
        // Arrange
        var test = await CreateTestWithRatings();

        // Act
        var invalidHighRating = test.AddRating(_testUserId, 6, TestsSharedConsts.DateTimeProviderInstance);

        // Assert
        Assert.True(invalidHighRating.IsErr(out var err));
        Assert.Equal(err.Code, Err.ErrCodes.InvalidData);
    }

    [Fact]
    public async Task AddRating_ShouldAddRatingAndRaiseEvent_WhenSucceed() {
        // Arrange
        var test = await CreateTestWithRatings();
        ushort ratingValue = 5;

        // Act
        var result = test.AddRating(_testUserId, ratingValue, TestsSharedConsts.DateTimeProviderInstance);

        // Assert
        Assert.False(result.IsErr());

        var rating = result.GetSuccess();
        Assert.Equal(_testUserId, rating.UserId);
        Assert.Equal(ratingValue, rating.Value);

        var ratingExists = test.GetRatingsPackage(0).GetSuccess()
            .Any(r => r.UserId == _testUserId && r.Value == ratingValue);
        Assert.True(ratingExists, "Rating should be added to the test");

        var eventExists = test.GetDomainEventsCopy().Any(e =>
            e is AppUserLeftTestRatingEvent evt && evt.UserId == _testUserId);
        Assert.True(eventExists, "Event should be raised when a rating is added");
    }
}