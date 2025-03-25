using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.tier_list_format.feedback;
using TestTakingService.Domain.Common.test_taken_data.tier_list_format_test;

namespace TestTakingService.Domain.UnitTests.TestAggregateRoot.tier_list_format;

public class TierListTestTakingFeedbackTests
{
    private static TierListTestFeedbackOption BasicFeedbackOption => TierListTestFeedbackOption.Enabled.CreateNew(
        AnonymityValues.Any, "text text text text text", 100
    ).GetSuccess();

    [Fact]
    public void TestTakenWithFeedbackLeft_WhenFeedbackDisabled_ShouldReturnErr() {
        // Arrange
        var test = TierListTestTestsConsts.CreateTest(feedbackOption: TierListTestFeedbackOption.Disabled.Instance);
        Dictionary<TierListTestTierId, TierListTestTakenTierData> itemsInTiers = new();

        TierListTestTakenFeedbackData feedback = new("Great test!", LeftAnonymously: false);

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            itemsInTiers,
            TestSharedTestsConsts.TestTakingStart,
            TestSharedTestsConsts.TestTakingEnd,
            feedback,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Feedback for this test is disabled", err.Message);
    }

    [Fact]
    public void TestTakenWithNonAnonymousFeedbackLeft_WhenMissingTestTakerId_ShouldReturnErr() {
        // Arrange
        var test = TierListTestTestsConsts.CreateTest(feedbackOption: BasicFeedbackOption);
        Dictionary<TierListTestTierId, TierListTestTakenTierData> itemsInTiers = new();

        TierListTestTakenFeedbackData feedback = new("Nice work!", LeftAnonymously: false);

        // Act
        var result = test.TestTaken(
            null, // Missing test taker ID
            itemsInTiers,
            TestSharedTestsConsts.TestTakingStart,
            TestSharedTestsConsts.TestTakingEnd,
            feedback,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("The feedback was left non-anonymously, but the user ID is not provided", err.Message);
    }

    [Fact]
    public void TestTakenWithAnonymousFeedbackLeft_WhenTestAcceptNonAnonymousOnly_ShouldReturnErr() {
        // Arrange
        var feedbackOption = TierListTestFeedbackOption.Enabled.CreateNew(
            AnonymityValues.NonAnonymousOnly, "text text text text text", 56
        ).GetSuccess();
        var test = TierListTestTestsConsts.CreateTest(feedbackOption: feedbackOption);
        Dictionary<TierListTestTierId, TierListTestTakenTierData> itemsInTiers = new();

        TierListTestTakenFeedbackData feedback =
            new("Interesting test!", LeftAnonymously: true); // Feedback is anonymous

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            itemsInTiers,
            TestSharedTestsConsts.TestTakingStart,
            TestSharedTestsConsts.TestTakingEnd,
            feedback,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("You are trying to leave anonymous feedback, but this test only supports non-anonymous feedback",
            err.Message);
    }

    [Fact]
    public void TestTakenWithNonAnonymousFeedbackLeft_WhenTestAcceptAnonymousOnly_ShouldReturnErr() {
        // Arrange
        var feedbackOption = TierListTestFeedbackOption.Enabled.CreateNew(
            AnonymityValues.AnonymousOnly, "text text text text text", 56
        ).GetSuccess(); // Feedback option is set to anonymous only
        var test = TierListTestTestsConsts.CreateTest(feedbackOption: feedbackOption);
        Dictionary<TierListTestTierId, TierListTestTakenTierData> itemsInTiers = new();

        TierListTestTakenFeedbackData feedback =
            new("Interesting test!", LeftAnonymously: false); // Feedback is non-anonymous

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            itemsInTiers,
            TestSharedTestsConsts.TestTakingStart,
            TestSharedTestsConsts.TestTakingEnd,
            feedback,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("You are trying to leave non-anonymous feedback, but this test only supports anonymous feedback",
            err.Message);
    }

    [Fact]
    public void TestTakenWithFeedbackLeft_WithEmptyFeedback_ShouldReturnErr() {
        // Arrange
        var test = TierListTestTestsConsts.CreateTest(feedbackOption: BasicFeedbackOption);
        Dictionary<TierListTestTierId, TierListTestTakenTierData> itemsInTiers = new();

        TierListTestTakenFeedbackData feedback = new(string.Empty, false); // Empty feedback

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            itemsInTiers,
            TestSharedTestsConsts.TestTakingStart,
            TestSharedTestsConsts.TestTakingEnd,
            feedback,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("You are trying to leave feedback, but it's empty", err.Message);
    }

    [Fact]
    public void TestTakenWithFeedbackLeft_WithTooLongFeedback_ShouldReturnErr() {
        // Arrange
        var test = TierListTestTestsConsts.CreateTest(feedbackOption: BasicFeedbackOption);

        Dictionary<TierListTestTierId, TierListTestTakenTierData> itemsInTiers = new();

        var longFeedbackText = new string('a', 150); // Too long feedback
        TierListTestTakenFeedbackData feedback = new(longFeedbackText, false);

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            itemsInTiers,
            TestSharedTestsConsts.TestTakingStart,
            TestSharedTestsConsts.TestTakingEnd,
            feedback,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Your feedback text is too long. Maximum length of feedback is 100", err.Message);
    }
}