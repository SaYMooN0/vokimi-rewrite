using SharedKernel.Common.domain.entity;
using SharedKernel.Common.general_test_questions;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;
using TestTakingService.Domain.Common.test_taken_data.general_format_test;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Domain.UnitTests.TestAggregateRoot.general_format;

public class GeneralTestTakingProvidedAnswersTests
{
    [Fact]
    public void TestTaken_DataForQuestionIsNotProvided_ShouldReturnErr() {
        // Arrange
        var test = GeneralTestTestsConsts.CreateTest();
        var providedMap = new Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> {
            [GeneralTestTestsConsts.Question1.Id] = new([GeneralTestTestsConsts.Q1Answer1Id], TimeSpan.FromSeconds(5))
            // Other questions missing
        };

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            providedMap,
            TestSharedTestsConsts.TestTakingStart,
            TestSharedTestsConsts.TestTakingEnd,
            null,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Answers for not all questions are chosen", err.Message);
    }

    [Fact]
    public void TestTaken_WhenTestDoesNotContainProvidedQuestion_ShouldReturnErr() {
        // Arrange
        var fakeQuestionId = new GeneralTestQuestionId(Guid.NewGuid());
        var test = GeneralTestTestsConsts.CreateTest(
            questions: [GeneralTestTestsConsts.Question1]
        );

        var providedMap = new Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> {
            [fakeQuestionId] = new(
                [GeneralTestTestsConsts.Q1Answer1Id],
                TimeSpan.FromSeconds(5)
            )
        };

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            providedMap,
            TestSharedTestsConsts.TestTakingStart,
            TestSharedTestsConsts.TestTakingEnd,
            null,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Answers for not all questions are chosen", err.Message);
    }

    [Fact]
    public void TestTaken_WhenChosenAnswersContainAnswersNotBelongingToQuestion_ShouldReturnErr() {
        // Arrange
        var test = GeneralTestTestsConsts.CreateTest(questions: [GeneralTestTestsConsts.Question1]);

        var wrongAnswerId = GeneralTestTestsConsts.Q2Answer1Id; // belongs to another question
        var providedMap = new Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> {
            [GeneralTestTestsConsts.Question1.Id] = new([wrongAnswerId], TimeSpan.FromSeconds(5))
        };

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            providedMap,
            TestSharedTestsConsts.TestTakingStart,
            TestSharedTestsConsts.TestTakingEnd,
            null,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Question has some answers marked as chosen that do not belong to this question", err.Message);
    }

    [Fact]
    public void TestTaken_WhenSameAnswerIsChosenMultipleTimes_ShouldReturnErr() {
        // Arrange
        var test = GeneralTestTestsConsts.CreateTest(questions: [GeneralTestTestsConsts.Question2]);

        var answerId = GeneralTestTestsConsts.Q2Answer1Id;
        var providedMap = new Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> {
            [GeneralTestTestsConsts.Question2.Id] = new(
                ChosenAnswers: [answerId, answerId], // duplicate
                TimeOnQuestionSpent: TimeSpan.FromSeconds(5)
            )
        };

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            providedMap,
            TestSharedTestsConsts.TestTakingStart,
            TestSharedTestsConsts.TestTakingEnd,
            null,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Question has some answers marked as chosen that do not belong to this question", err.Message);
    }

    [Fact]
    public void TestTaken_WhenChosenAnswersCountLessThanQuestionMinLimit_ShouldReturnErr() {
        // Arrange
        var q = new GeneralTestQuestion(
            GeneralTestQuestionId.CreateNew(),
            orderInTest: 1,
            text: "Pick at least 2",
            images: [],
            answersType: GeneralTestAnswersType.TextOnly,
            answers: [
                new GeneralTestAnswer(GeneralTestAnswerId.CreateNew(),
                    GeneralTestAnswerTypeSpecificData.TextOnly.CreateNew("A").GetSuccess(), []),
                new GeneralTestAnswer(GeneralTestAnswerId.CreateNew(),
                    GeneralTestAnswerTypeSpecificData.TextOnly.CreateNew("B").GetSuccess(), [])
            ],
            shuffleAnswers: false,
            answersCountLimit: GeneralTestQuestionAnswersCountLimit.MultipleChoice(2, 2).GetSuccess(),
            timeLimit: GeneralTestQuestionTimeLimitOption.NoTimeLimit()
        );

        var test = GeneralTestTestsConsts.CreateTest(questions: [q]);

        var providedMap = new Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> {
            [q.Id] = new([q.Answers.First().Id], TimeSpan.FromSeconds(3))
            // only 1 selected, min is 2
        };

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            providedMap,
            TestSharedTestsConsts.TestTakingStart,
            TestSharedTestsConsts.TestTakingEnd,
            null,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Contains("at least", err.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void TestTaken_WhenChosenAnswersCountGreaterThanQuestionMaxLimit_ShouldReturnErr() {
        // Arrange
        var q = new GeneralTestQuestion(
            GeneralTestQuestionId.CreateNew(),
            orderInTest: 1,
            text: "Pick at most 1",
            images: [],
            answersType: GeneralTestAnswersType.TextOnly,
            answers: [
                new GeneralTestAnswer(GeneralTestAnswerId.CreateNew(),
                    GeneralTestAnswerTypeSpecificData.TextOnly.CreateNew("A").GetSuccess(), []),
                new GeneralTestAnswer(GeneralTestAnswerId.CreateNew(),
                    GeneralTestAnswerTypeSpecificData.TextOnly.CreateNew("B").GetSuccess(), []),
                new GeneralTestAnswer(GeneralTestAnswerId.CreateNew(),
                    GeneralTestAnswerTypeSpecificData.TextOnly.CreateNew("C").GetSuccess(), [])
            ],
            shuffleAnswers: false,
            answersCountLimit: GeneralTestQuestionAnswersCountLimit.MultipleChoice(1, 1).GetSuccess(),
            timeLimit: GeneralTestQuestionTimeLimitOption.NoTimeLimit()
        );

        var test = GeneralTestTestsConsts.CreateTest(questions: [q]);
        var answers = q.Answers.ToArray();
        var providedMap = new Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> {
            [q.Id] = new(
                [answers[0].Id, answers[1].Id],
                // max 1 allowed, provided 2
                TimeSpan.FromSeconds(3)
            )
        };

        // Act
        var result = test.TestTaken(
            TestSharedTestsConsts.TestTakerId,
            providedMap,
            TestSharedTestsConsts.TestTakingStart,
            TestSharedTestsConsts.TestTakingEnd,
            null,
            TestSharedTestsConsts.DateTimeProviderInstance
        );

        // Assert
        Assert.True(result.IsErr(out var err));
    }
}