using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestCatalogService.Domain.AppUserAggregate.events;
using TestCatalogService.Domain.TestAggregate;

namespace TestCatalogService.Domain.UnitTests.TestAggregateRoot.formats_shared;

public class TestRatingsTests
{
    private readonly AppUserId _testUserId = new(Guid.NewGuid());

    private BaseTest CreateTestWithRatings(Dictionary<AppUserId, ushort>? initialRatings = null) {
        var test = TestsSharedTestsConsts.CreateBaseTest();

        if (initialRatings != null) {
            foreach (var (userId, rating) in initialRatings) {
                var ratingRes = test.AddRating(userId, rating, TestsSharedConsts.DateTimeProviderInstance);
                Assert.False(ratingRes.IsErr(), "Failed to initialize test with ratings.");
            }
        }

        return test;
    }

    [Fact]
    public void AddRating_ShouldReturnError_WhenUserHasAlreadyRated() {
        // Arrange
        var initialRatings = new Dictionary<AppUserId, ushort> { { _testUserId, 4 } };
        var test = CreateTestWithRatings(initialRatings);

        // Act
        var result = test.AddRating(_testUserId, 5, TestsSharedConsts.DateTimeProviderInstance);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Contains("This user has already rated this test", err.Message);
    }

    [Fact]
    public void AddRating_ShouldReturnError_WhenRatingIsOutOfRange() {
        // Arrange
        var test = CreateTestWithRatings();

        // Act
        var invalidHighRating = test.AddRating(_testUserId, 6, TestsSharedConsts.DateTimeProviderInstance);

        // Assert
        Assert.True(invalidHighRating.IsErr(out var err));
        Assert.Equal(err.Code, Err.ErrCodes.InvalidData);
    }

    [Fact]
    public void AddRating_ShouldAddRatingAndRaiseEvent_WhenSuccessful() {
        // Arrange
        var test =  CreateTestWithRatings();
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