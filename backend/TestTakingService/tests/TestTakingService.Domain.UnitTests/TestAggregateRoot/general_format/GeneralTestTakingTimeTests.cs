using SharedKernel.Common.domain.entity;
using TestTakingService.Domain.Common.test_taken_data.general_format_test;

namespace TestTakingService.Domain.UnitTests.TestAggregateRoot.general_format;

public class GeneralTestTakingTimeTests
{
    [Fact]
    public void TestTaken_StartTimeLaterThanEndTime_ShouldReturnErr() {
        // Arrange
        var test = GeneralTestTestsConsts.CreateTest();
        Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> questionsDataMap = new();
        GeneralTestTakenFeedbackData feedback = new("Great test!", LeftAnonymously: false);

        var startTime = GeneralTestTestsConsts.DateTimeProviderInstance.Now.AddMinutes(10);
        var endTime = GeneralTestTestsConsts.DateTimeProviderInstance.Now.AddMinutes(5);

        // Act
        var result = test.TestTaken(
            GeneralTestTestsConsts.TestTakerId,
            questionsDataMap,
            startTime,
            endTime,
            feedback,
            GeneralTestTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Test start time cannot be later than end time", err.Message);
    }

    [Fact]
    public void TestTaken_StartTimeInTheFuture_ShouldReturnErr() {
        // Arrange
        var test = GeneralTestTestsConsts.CreateTest();
        Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> questionsDataMap = new();
        GeneralTestTakenFeedbackData feedback = new("Great test!", LeftAnonymously: false);

        DateTime startTime = GeneralTestTestsConsts.DateTimeProviderInstance.Now.AddMinutes(5);
        DateTime endTime = GeneralTestTestsConsts.DateTimeProviderInstance.Now.AddMinutes(10);

        // Act
        var result = test.TestTaken(
            GeneralTestTestsConsts.TestTakerId,
            questionsDataMap,
            startTime,
            endTime,
            feedback,
            GeneralTestTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Test couldn't start in a future", err.Message);
    }

    [Fact]
    public void TestTaken_TotalTimeSpentOnQuestionsExceedsTestDuration_ShouldReturnErr() {
        // Arrange
        var test = GeneralTestTestsConsts.CreateTest();
        Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> questionsDataMap = new() {
            {
                new(Guid.NewGuid()),
                new GeneralTestTakenQuestionData([], TimeSpan.FromMinutes(30))
            }, {
                new(Guid.NewGuid()),
                new GeneralTestTakenQuestionData([], TimeSpan.FromMinutes(40))
            }
        };

        var testTakingStart = GeneralTestTestsConsts.DateTimeProviderInstance.Now.AddMinutes(-30); 
        var testTakingEnd = GeneralTestTestsConsts.DateTimeProviderInstance.Now.AddMinutes(-5); 

        // Act
        var result = test.TestTaken(
            GeneralTestTestsConsts.TestTakerId,
            questionsDataMap,
            testTakingStart,
            testTakingEnd,
            null,
            GeneralTestTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Somehow total time spent on questions exceeds the total test duration", err.Message);
    }
}