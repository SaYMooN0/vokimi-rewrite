using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects;
using TestCatalogService.Domain.TestAggregate;
using TestCatalogService.Domain.TestAggregate.formats_shared;
using TestCatalogService.Domain.UnitTests.FakeRepositories;

namespace TestCatalogService.Domain.UnitTests.TestAggregateRoot.formats_shared.ratings;

public class UpdatingTestRatingTest
{
    private static DateTime RndDate => DateTime.Now
        .AddHours(Random.Shared.Next(-10, -2))
        .AddMonths(Random.Shared.Next(-8, -2));

    private static readonly AppUserId _userIdToUpdateRating = new(Guid.NewGuid());

    private readonly FakeUserFollowingsRepository _userFollowingsRepository = new([
        new(followerId: _userIdToUpdateRating, AppUserId.CreateNew(), RndDate),
        new(followerId: _userIdToUpdateRating, AppUserId.CreateNew(), RndDate),
        new(followerId: _userIdToUpdateRating, AppUserId.CreateNew(), RndDate),
        new(followerId: AppUserId.CreateNew(), _userIdToUpdateRating, RndDate),
        new(followerId: AppUserId.CreateNew(), _userIdToUpdateRating, RndDate)
    ]);

    private async Task<BaseTest> CreateTestWithRatings(
        Dictionary<AppUserId, ushort>? initialRatings = null,
        TestInteractionsAccessSettings? customInteractionsAccessSettings = null
    ) {
        var test = TestsSharedTestsConsts.CreateBaseTest(customInteractionsAccessSettings);

        if (initialRatings != null) {
            foreach (var (userId, rating) in initialRatings) {
                var ratingRes = await test.AddRating(
                    userId, rating, _userFollowingsRepository, TestsSharedTestsConsts.DateTimeProviderInstance
                );
                Assert.False(ratingRes.IsErr(), "Failed to initialize test with ratings.");
            }
        }

        return test;
    }

    [Fact]
    public async Task UpdateRating_WhenUserHasNotRated_ShouldReturnError() {
        // Arrange
        var test = await CreateTestWithRatings();

        // Act
        var result = await test.UpdateRating(_userIdToUpdateRating, 4, _userFollowingsRepository,
            TestsSharedTestsConsts.DateTimeProviderInstance);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Contains("Cannot update rating because user has not rated this test", err.Message);
    }

    [Fact]
    public async Task UpdateRating_WhenUserHasAlreadyRated_ShouldSucceed() {
        // Arrange
        var initialRatings = new Dictionary<AppUserId, ushort> { { _userIdToUpdateRating, 3 } };
        var test = await CreateTestWithRatings(initialRatings);
        ushort newVal = 5;

        // Act
        var result = await test.UpdateRating(
            _userIdToUpdateRating,
            newVal,
            _userFollowingsRepository,
            TestsSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.False(result.IsErr());
        Assert.Equal(5, result.GetSuccess().Value);
        var ratingExists = test
            .GetRatingsPackage(0)
            .GetSuccess()
            .Any(r => r.UserId == _userIdToUpdateRating && r.Value == newVal);
        Assert.True(ratingExists);
    }

    [Fact]
    public async Task UpdateRating_WhenRatingsAreDisabled_ShouldReturnError() {
        // Arrange
        var test = await CreateTestWithRatings(customInteractionsAccessSettings: new TestInteractionsAccessSettings(
            AccessLevel.Public,
            ResourceAvailabilitySetting.Disabled,
            ResourceAvailabilitySetting.EnabledPublic,
            allowTagsSuggestions: false,
            allowTestTakenPosts: true
        ));

        // Act
        var result = await test.UpdateRating(
            _userIdToUpdateRating,
            4,
            _userFollowingsRepository,
            TestsSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal(err.Code, Err.ErrCodes.NoAccess);
        Assert.Contains("Ratings are disabled for this test", err.Message);
    }

    [Fact]
    public async Task UpdateRating_WhenRatingsArePrivateAndUserIsNotCreator_ShouldReturnError() {
        // Arrange
        var initialRatings = new Dictionary<AppUserId, ushort> { { _userIdToUpdateRating, 3 } };
        var test = await CreateTestWithRatings(initialRatings, new TestInteractionsAccessSettings(
            AccessLevel.Private,
            ResourceAvailabilitySetting.EnabledPrivate,
            ResourceAvailabilitySetting.EnabledPublic,
            allowTagsSuggestions: false,
            allowTestTakenPosts: true
        ));

        // Act
        var result = await test.UpdateRating(
            _userIdToUpdateRating,
            4,
            _userFollowingsRepository,
            TestsSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal(err.Code, Err.ErrCodes.NoAccess);
        Assert.Contains("Test ratings are private. You must be the creator or editor to rate", err.Message);
    }

    [Fact]
    public async Task UpdateRating_WhenRatingsArePrivateAndUserIsCreator_ShouldSucceed() {
        // Arrange
        var initialRatings = new Dictionary<AppUserId, ushort> { { TestsSharedTestsConsts.TestCreatorId, 3 } };
        var test = await CreateTestWithRatings(initialRatings, new TestInteractionsAccessSettings(
            AccessLevel.Private,
            ResourceAvailabilitySetting.EnabledPrivate,
            ResourceAvailabilitySetting.EnabledPublic,
            allowTagsSuggestions: false,
            allowTestTakenPosts: true
        ));

        // Act
        var result = await test.UpdateRating(
            TestsSharedTestsConsts.TestCreatorId,
            5,
            _userFollowingsRepository,
            TestsSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.False(result.IsErr());
        Assert.Equal(5, result.GetSuccess().Value);
    }

    [Fact]
    public async Task UpdateRating_WhenRatingsAreOnlyForFollowersAndUserIsNotAFollower_ShouldReturnError() {
        // Arrange
        var nonFollowerId = AppUserId.CreateNew();
        var initialRatings = new Dictionary<AppUserId, ushort> { { nonFollowerId, 3 } };
        var test = await CreateTestWithRatings(initialRatings, new TestInteractionsAccessSettings(
            AccessLevel.Public,
            ResourceAvailabilitySetting.EnabledFollowersOnly,
            ResourceAvailabilitySetting.EnabledPublic,
            allowTagsSuggestions: false,
            allowTestTakenPosts: true
        ));

        // Act
        var result = await test.UpdateRating(nonFollowerId, 5, _userFollowingsRepository,
            TestsSharedTestsConsts.DateTimeProviderInstance);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal(err.Code, Err.ErrCodes.NoAccess);
        Assert.Contains("You must be following the test creator to rate this test", err.Message);
    }

    [Fact]
    public async Task UpdateRating_WhenRatingsAreOnlyForFollowersAndUserIsAFollower_ShouldSucceed() {
        // Arrange
        var initialRatings = new Dictionary<AppUserId, ushort> { { _userIdToUpdateRating, 3 } };
        var test = await CreateTestWithRatings(initialRatings, new TestInteractionsAccessSettings(
            AccessLevel.Public,
            ResourceAvailabilitySetting.EnabledFollowersOnly,
            ResourceAvailabilitySetting.EnabledPublic,
            allowTagsSuggestions: false,
            allowTestTakenPosts: true
        ));

        // Act
        var result = await test.UpdateRating(
            _userIdToUpdateRating,
            5,
            ,
            TestsSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.False(result.IsErr());
        Assert.Equal(5, result.GetSuccess().Value);
    }

    [Fact]
    public async Task UpdateRating_WhenNewRatingIsOutOfRange_ShouldReturnError() {
        // Arrange
        var initialRatings = new Dictionary<AppUserId, ushort> { { _userIdToUpdateRating, 3 } };
        var test = await CreateTestWithRatings(initialRatings);

        // Act
        var result = await test.UpdateRating(
            _userIdToUpdateRating,
            6
            , _userFollowingsRepository,
            TestsSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal(Err.ErrCodes.InvalidData, err.Code);
    }

    [Fact]
    public async Task UpdateRating_WhenNewRatingIsSameAsPrevious_ShouldSucceedWithoutChange() {
        // Arrange
        var initialRatings = new Dictionary<AppUserId, ushort> { { _userIdToUpdateRating, 4 } };
        var test = await CreateTestWithRatings(initialRatings);

        // Act
        var result = await test.UpdateRating(
            _userIdToUpdateRating,
            4,
            _userFollowingsRepository,
            TestsSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.False(result.IsErr());
        Assert.Equal(4, result.GetSuccess().Value);
    }
}