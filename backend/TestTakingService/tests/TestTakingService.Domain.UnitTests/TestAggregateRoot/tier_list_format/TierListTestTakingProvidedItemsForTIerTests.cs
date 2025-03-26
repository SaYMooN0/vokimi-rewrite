using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.tier_list_format;
using TestTakingService.Domain.Common.test_taken_data.tier_list_format_test;
using TestTakingService.Domain.TestAggregate.tier_list_format;

namespace TestTakingService.Domain.UnitTests.TestAggregateRoot.tier_list_format;

public class TierListTestTakingProvidedItemsForTIerTests
{
    [Fact]
    public void TestTaken_WhenTierItemsDataIsMissingForOneOfTheTiers_ShouldReturnErr() {
        // Arrange
        var test = TierListTestTestsConsts.CreateTest(
            tiers: [TierListTestTestsConsts.TierA, TierListTestTestsConsts.TierB]
        );
        var tierData = new TierListTestTakenTierData(1) {
            [TierListTestTestsConsts.Item1Id] = 0
        };

        var itemsInTiers = new Dictionary<TierListTestTierId, TierListTestTakenTierData> {
            [TierListTestTestsConsts.TierAId] = tierData
            // Missing TierB data
        };

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            itemsInTiers,
            TestSharedTestsConsts.TestTakingStart,
            TestSharedTestsConsts.TestTakingEnd,
            null,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Data about items is not provided for all the tiers", err.Message);
    }

    [Fact]
    public void TestTaken_WhenTierItemCountExceedsMaxLimit_ShouldReturnErr() {
        // Arrange
        TierListTestTier TierA = new(
            TierListTestTestsConsts.TierAId,
            name: "Top Tier",
            description: "The best items",
            maxItemsCountLimit: 1,
            styles: TierListTestTierStyles.Default(),
            orderInTest: 0
        );
        var test = TierListTestTestsConsts.CreateTest(
            tiers: [TierA, TierListTestTestsConsts.TierB]
        );

        var tierData = new TierListTestTakenTierData(4) {
            [TierListTestTestsConsts.Item1Id] = 0,
            [TierListTestTestsConsts.Item2Id] = 1,
            [TierListTestTestsConsts.Item3Id] = 2,
            [TierListTestTestsConsts.Item4Id] = 3 // exceeds TierA's limit of 1
        };

        var itemsInTiers = new Dictionary<TierListTestTierId, TierListTestTakenTierData> {
            [TierListTestTestsConsts.TierAId] = tierData,
            [TierListTestTestsConsts.TierBId] = new TierListTestTakenTierData(0)
        };

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            itemsInTiers,
            TestSharedTestsConsts.TestTakingStart,
            TestSharedTestsConsts.TestTakingEnd,
            null,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Contains("Items count limit in tier", err.Message);
    }

    [Fact]
    public void TestTaken_WhenItemIsNotPartOfTheTest_ShouldReturnErr() {
        // Arrange

        var test = TierListTestTestsConsts.CreateTest();
        var idOfItemNotInTest = TierListTestItemId.CreateNew();
        var tierData = new TierListTestTakenTierData(1) {
            [idOfItemNotInTest] = 0
        };

        var itemsInTiers = new Dictionary<TierListTestTierId, TierListTestTakenTierData> {
            [TierListTestTestsConsts.TierAId] = tierData,
            [TierListTestTestsConsts.TierBId] = new(0)
        };

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            itemsInTiers,
            TestSharedTestsConsts.TestTakingStart,
            TestSharedTestsConsts.TestTakingEnd,
            null,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("One of the provided items is not presented in this test", err.Message);
    }

    [Fact]
    public void TestTaken_WhenItemOrderIsIncorrectWithinTier_ShouldReturnErr() {
        // Arrange
        var test = TierListTestTestsConsts.CreateTest();

        var tierData = new TierListTestTakenTierData(2) {
            [TierListTestTestsConsts.Item1Id] = 1,
            [TierListTestTestsConsts.Item2Id] = 2 // incorrect order (should be 0 and 1)
        };

        var itemsInTiers = new Dictionary<TierListTestTierId, TierListTestTakenTierData> {
            [TierListTestTestsConsts.TierAId] = tierData,
            [TierListTestTestsConsts.TierBId] = new TierListTestTakenTierData(0)
        };

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            itemsInTiers,
            TestSharedTestsConsts.TestTakingStart,
            TestSharedTestsConsts.TestTakingEnd,
            null,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.StartsWith("Provided item order for item", err.Message);
    }

    [Fact]
    public void TestTaken_WhenItemIsPlacedInMultipleTiers_ShouldReturnErr() {
        // Arrange
        var test = TierListTestTestsConsts.CreateTest();

        var itemsInTiers = new Dictionary<TierListTestTierId, TierListTestTakenTierData> {
            [TierListTestTestsConsts.TierAId] = new() {
                [TierListTestTestsConsts.Item1Id] = 0
            },
            [TierListTestTestsConsts.TierBId] = new() {
                [TierListTestTestsConsts.Item1Id] = 0 // same item in both tiers
            }
        };

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            itemsInTiers,
            TestSharedTestsConsts.TestTakingStart,
            TestSharedTestsConsts.TestTakingEnd,
            null,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Contains("was provided in two or more different tiers", err.Message);
    }

    [Fact]
    public void TestTaken_WhenTotalProvidedItemsCountDoesNotMatchTestItemsCount_ShouldReturnErr() {
        // Arrange
        var test = TierListTestTestsConsts.CreateTest(
            items: [
                TierListTestTestsConsts.Item1,
                TierListTestTestsConsts.Item2,
                TierListTestTestsConsts.Item3,
                TierListTestTestsConsts.Item4
            ]
        );

        var itemsInTiers = new Dictionary<TierListTestTierId, TierListTestTakenTierData> {
            [TierListTestTestsConsts.TierAId] = new() {
                [TierListTestTestsConsts.Item1Id] = 0
            },
            [TierListTestTestsConsts.TierBId] = new() {
                [TierListTestTestsConsts.Item2Id] = 0
                // Missing Item3 and Item4
            }
        };

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            itemsInTiers,
            TestSharedTestsConsts.TestTakingStart,
            TestSharedTestsConsts.TestTakingEnd,
            null,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Incorrect count of provided items", err.Details);
    }
}