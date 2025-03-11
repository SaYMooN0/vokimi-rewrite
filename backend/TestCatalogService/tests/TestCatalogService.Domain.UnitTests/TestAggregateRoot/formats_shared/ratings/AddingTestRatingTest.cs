using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects;
using TestCatalogService.Domain.AppUserAggregate.events;
using TestCatalogService.Domain.TestAggregate;
using TestCatalogService.Domain.TestAggregate.formats_shared;
using TestCatalogService.Domain.UnitTests.FakeRepositories;

namespace TestCatalogService.Domain.UnitTests.TestAggregateRoot.formats_shared.ratings;

public class AddingTestRatingTest
{
    private static DateTime RndDate => DateTime.Now
        .AddHours(Random.Shared.Next(-10, -2))
        .AddMonths(Random.Shared.Next(-8, -2));

    private static readonly AppUserId _userIdToAddRating = new(Guid.NewGuid());

    private readonly FakeUserFollowingsRepository _userFollowingsRepository = new([
        new(followerId: _userIdToAddRating, AppUserId.CreateNew(), RndDate),
        new(followerId: _userIdToAddRating, AppUserId.CreateNew(), RndDate),
        new(followerId: _userIdToAddRating, AppUserId.CreateNew(), RndDate),
        new(followerId: AppUserId.CreateNew(), _userIdToAddRating, RndDate),
        new(followerId: AppUserId.CreateNew(), _userIdToAddRating, RndDate),
        new(followerId: AppUserId.CreateNew(), _userIdToAddRating, RndDate),
        new(followerId: AppUserId.CreateNew(), _userIdToAddRating, RndDate)
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
    public async Task AddRating_WhenUserHasAlreadyRated_ShouldReturnError() {
        // Arrange
        var initialRatings = new Dictionary<AppUserId, ushort> { { _userIdToAddRating, 4 } };
        var test = await CreateTestWithRatings(initialRatings);

        // Act
        var result = await test.AddRating(
            _userIdToAddRating, 5, _userFollowingsRepository,
            TestsSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Contains("This user has already rated this test", err.Message);
    }

    [Fact]
    public async Task AddRating_WhenRatingIsOutOfRange_ShouldReturnError() {
        // Arrange
        var test = await CreateTestWithRatings();

        // Act
        var invalidHighRating = await test.AddRating(
            _userIdToAddRating, 6, _userFollowingsRepository,
            TestsSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(invalidHighRating.IsErr(out var err));
        Assert.Equal(Err.ErrCodes.InvalidData, err.Code);
    }

    [Fact]
    public async Task AddRating_WhenRatingsAreDisabled_ShouldReturnError() {
        // Arrange
        var test = await CreateTestWithRatings(customInteractionsAccessSettings: new TestInteractionsAccessSettings(
                AccessLevel.Public,
                ResourceAvailabilitySetting.Disabled,
                ResourceAvailabilitySetting.EnabledPublic,
                allowTagsSuggestions: false,
                allowTestTakenPosts: true
            )
        );

        // Act
        var result = await test.AddRating(_userIdToAddRating, 5, _userFollowingsRepository,
            TestsSharedTestsConsts.DateTimeProviderInstance);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal(err.Code, Err.ErrCodes.NoAccess);
        Assert.Contains("Ratings are disabled for this test", err.Message);
    }

    [Fact]
    public async Task AddRating_WhenEverythingIsCorrect_WhenSucceedAndRaiseEvent() {
        // Arrange
        var test = await CreateTestWithRatings();
        ushort ratingValue = 5;

        // Act
        var result = await test.AddRating(_userIdToAddRating, ratingValue, _userFollowingsRepository,
            TestsSharedTestsConsts.DateTimeProviderInstance);

        // Assert
        Assert.False(result.IsErr());

        var rating = result.GetSuccess();
        Assert.Equal(_userIdToAddRating, rating.UserId);
        Assert.Equal(ratingValue, rating.Value);

        var ratingExists = test
            .GetRatingsPackage(0)
            .GetSuccess()
            .Any(r => r.UserId == _userIdToAddRating && r.Value == ratingValue);
        Assert.True(ratingExists);

        var eventExists = test.GetDomainEventsCopy().Any(e =>
            e is AppUserLeftTestRatingEvent evt && evt.UserId == _userIdToAddRating);
        Assert.True(eventExists, "Event should be raised when a rating is added");
    }

    [Fact]
    public async Task AddRating_WhenRatingsArePrivate_ShouldReturnError() {
        // Arrange
        var test = await CreateTestWithRatings(customInteractionsAccessSettings: new TestInteractionsAccessSettings(
                AccessLevel.Private,
                ResourceAvailabilitySetting.EnabledPrivate,
                ResourceAvailabilitySetting.EnabledPublic,
                allowTagsSuggestions: false,
                allowTestTakenPosts: true
            )
        );

        // Act
        var result = await test.AddRating(
            _userIdToAddRating,
            5,
            _userFollowingsRepository,
            TestsSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal(err.Code, Err.ErrCodes.NoAccess);
        Assert.Contains("Test ratings are private. You must be the creator or editor to rate", err.Message);
    }

    [Fact]
    public async Task AddRating_WhenRatingsArePrivateAndUserToRateIsCreator_ShouldSucceed() {
        // Arrange
        var test = await CreateTestWithRatings(customInteractionsAccessSettings: new TestInteractionsAccessSettings(
                AccessLevel.Private,
                ResourceAvailabilitySetting.EnabledPrivate,
                ResourceAvailabilitySetting.EnabledPublic,
                allowTagsSuggestions: false,
                allowTestTakenPosts: true
            )
        );

        // Act
        var result = await test.AddRating(
            TestsSharedTestsConsts.TestCreatorId,
            5,
            _userFollowingsRepository,
            TestsSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.False(result.IsErr());
    }

    [Fact]
    public async Task AddRating_WhenRatingsAreOnlyForFollowersAndDoesNotFollows_ShouldReturnError() {
        // Arrange
        var nonFollowerId = AppUserId.CreateNew();
        var test = await CreateTestWithRatings(customInteractionsAccessSettings: new TestInteractionsAccessSettings(
                AccessLevel.Public,
                ResourceAvailabilitySetting.EnabledFollowersOnly,
                ResourceAvailabilitySetting.EnabledPublic,
                allowTagsSuggestions: false,
                allowTestTakenPosts: true
            )
        );

        // Act
        var result = await test.AddRating(nonFollowerId, 5, _userFollowingsRepository,
            TestsSharedTestsConsts.DateTimeProviderInstance);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal(err.Code, Err.ErrCodes.NoAccess);
        Assert.Contains("You must be following the test creator to rate this test", err.Message);
    }

    [Fact]
    public async Task AddRating_WhenRatingsAreOnlyForFollowersAndFollows_ShouldSucceed() {
        // Arrange
        var userFollowingsRepository = new FakeUserFollowingsRepository([
            new(
                followerId: _userIdToAddRating,
                followedUserId: TestsSharedTestsConsts.TestCreatorId,
                RndDate
            )
        ]);
        var test = await CreateTestWithRatings(
            customInteractionsAccessSettings: new TestInteractionsAccessSettings(
                AccessLevel.Public,
                ResourceAvailabilitySetting.EnabledFollowersOnly,
                ResourceAvailabilitySetting.EnabledPublic,
                allowTagsSuggestions: false,
                allowTestTakenPosts: true
            )
        );

        // Act
        var result = await test.AddRating(
            _userIdToAddRating,
            5,
            userFollowingsRepository,
            TestsSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsSuccess());
    }
}