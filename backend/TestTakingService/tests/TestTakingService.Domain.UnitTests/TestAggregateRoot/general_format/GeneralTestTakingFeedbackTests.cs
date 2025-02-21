using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.interfaces;
using SharedKernel.Common.tests.general_format;
using TestTakingService.Domain.Common.general_test_taken_data;

namespace TestTakingService.Domain.UnitTests.TestAggregateRoot.general_format;

public class GeneralTestTakingFeedbackTests
{
    private static GeneralTestFeedbackOption BasicFeedbackOption => GeneralTestFeedbackOption.Enabled.CreateNew(
        AnonymityValues.Any, "text text text text text", 100
    ).GetSuccess();

    [Fact]
    public void TestTaken_FeedbackDisabled_ReturnsError() {
        // Arrange
        var test = GeneralTestTestsConsts.CreateTest(feedbackOption: GeneralTestFeedbackOption.Disabled.Instance);
        Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> questionsDataMap = new();
        GeneralTestTakenFeedbackData feedback = new("Great test!", LeftAnonymously: false);

        // Act
        var result = test.TestTaken(
            GeneralTestTestsConsts.TestTakerId,
            questionsDataMap,
            GeneralTestTestsConsts.TestTakingStart,
            GeneralTestTestsConsts.TestTakingEnd,
            feedback,
            GeneralTestTestsConsts.DateTimeProviderInstance
        );
    
        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Feedback for this test is disabled", err.Message);
    }

    [Fact]
    public void TestTaken_MissingTestTakerIdNonAnonymousFeedback_ReturnsError() {
        // Arrange
        var test = GeneralTestTestsConsts.CreateTest(feedbackOption: BasicFeedbackOption);
        Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> questionsDataMap = new();
        GeneralTestTakenFeedbackData feedback = new("Nice work!", LeftAnonymously: false);

        // Act
        var result = test.TestTaken(
            null, // Missing test taker ID
            questionsDataMap,
            GeneralTestTestsConsts.TestTakingStart,
            GeneralTestTestsConsts.TestTakingEnd,
            feedback,
            GeneralTestTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("The feedback was left non-anonymously, but the user ID is not provided", err.Message);
    }

    [Fact]
    public void TestTaken_AnonymityMismatch_AnonymousFeedback_ReturnsError() {
        // Arrange
        var feedbackOption = GeneralTestFeedbackOption.Enabled.CreateNew(
            AnonymityValues.NonAnonymousOnly, "text text text text text", 56
        ).GetSuccess();
        var test = GeneralTestTestsConsts.CreateTest(feedbackOption: feedbackOption);
        Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> questionsDataMap = new();
        GeneralTestTakenFeedbackData feedback =
            new("Interesting test!", LeftAnonymously: true); // Feedback is anonymous

        // Act
        var result = test.TestTaken(
            GeneralTestTestsConsts.TestTakerId,
            questionsDataMap,
            GeneralTestTestsConsts.TestTakingStart,
            GeneralTestTestsConsts.TestTakingEnd,
            feedback,
            GeneralTestTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("You are trying to leave anonymous feedback, but this test only supports non-anonymous feedback",
            err.Message);
    }

    [Fact]
    public void TestTaken_AnonymityMismatch_NonAnonymousFeedback_ReturnsError() {
        // Arrange
        var feedbackOption = GeneralTestFeedbackOption.Enabled.CreateNew(
            AnonymityValues.AnonymousOnly, "text text text text text", 56
        ).GetSuccess(); // Feedback option is set to anonymous only
        var test = GeneralTestTestsConsts.CreateTest(feedbackOption: feedbackOption);
        Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> questionsDataMap = new();
        GeneralTestTakenFeedbackData feedback =
            new("Interesting test!", LeftAnonymously: false); // Feedback is non-anonymous

        // Act
        var result = test.TestTaken(
            GeneralTestTestsConsts.TestTakerId,
            questionsDataMap,
            GeneralTestTestsConsts.TestTakingStart,
            GeneralTestTestsConsts.TestTakingEnd,
            feedback,
            GeneralTestTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("You are trying to leave non-anonymous feedback, but this test only supports anonymous feedback",
            err.Message);
    }

    [Fact]
    public void TestTaken_FeedbackLengthTooShort_ReturnsError() {
        // Arrange
        var test = GeneralTestTestsConsts.CreateTest(feedbackOption: BasicFeedbackOption);
        Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> questionsDataMap = new();
        GeneralTestTakenFeedbackData feedback = new(string.Empty, false); // Empty feedback

        // Act
        var result = test.TestTaken(
            GeneralTestTestsConsts.TestTakerId,
            questionsDataMap,
            GeneralTestTestsConsts.TestTakingStart,
            GeneralTestTestsConsts.TestTakingEnd,
            feedback,
            GeneralTestTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("You are trying to leave feedback, but it's empty", err.Message);
    }

    [Fact]
    public void TestTaken_FeedbackLengthTooLong_ReturnsError() {
        // Arrange
        var test = GeneralTestTestsConsts.CreateTest(feedbackOption: BasicFeedbackOption);

        Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> questionsDataMap = new();
        var longFeedbackText = new string('a', 150); // Too long feedback
        GeneralTestTakenFeedbackData feedback = new(longFeedbackText, false);

        // Act
        var result = test.TestTaken(
            GeneralTestTestsConsts.TestTakerId,
            questionsDataMap,
            GeneralTestTestsConsts.TestTakingStart,
            GeneralTestTestsConsts.TestTakingEnd,
            feedback,
            GeneralTestTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Your feedback text is too long. Maximum length of feedback is 100", err.Message);
    }
}