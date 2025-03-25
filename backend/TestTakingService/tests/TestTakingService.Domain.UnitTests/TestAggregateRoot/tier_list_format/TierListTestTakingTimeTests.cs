using SharedKernel.Common.domain.entity;
using TestTakingService.Domain.Common.test_taken_data.tier_list_format_test;

namespace TestTakingService.Domain.UnitTests.TestAggregateRoot.tier_list_format;

public class TierListTestTakingTimeTests
{
    [Fact]
    public void TestTaken_StartTimeLaterThanEndTime_ShouldReturnErr() {
        // Arrange
        var test = TierListTestTestsConsts.CreateTest();
        TierListTestTakenFeedbackData feedback = new("Great test!", LeftAnonymously: false);
        Dictionary<TierListTestTierId, TierListTestTakenTierData> itemsInTiers = new();


        var startTime = TestSharedTestsConsts.DateTimeProviderInstance.Now.AddMinutes(10);
        var endTime = TestSharedTestsConsts.DateTimeProviderInstance.Now.AddMinutes(5);

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            itemsInTiers,
            testTakingStart: startTime,
            testTakingEnd: endTime,
            feedback,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Test start time cannot be later than end time", err.Message);
    }

    [Fact]
    public void TestTaken_StartTimeInTheFuture_ShouldReturnErr() {
        // Arrange
        var test = TierListTestTestsConsts.CreateTest();
        TierListTestTakenFeedbackData feedback = new("Great test!", LeftAnonymously: false);
        Dictionary<TierListTestTierId, TierListTestTakenTierData> itemsInTiers = new();


        DateTime startTime = TestSharedTestsConsts.DateTimeProviderInstance.Now.AddMinutes(5);
        DateTime endTime = TestSharedTestsConsts.DateTimeProviderInstance.Now.AddMinutes(10);

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            itemsInTiers,
            testTakingStart: startTime,
            testTakingEnd: endTime,
            feedback,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Test couldn't start in a future", err.Message);
    }
}