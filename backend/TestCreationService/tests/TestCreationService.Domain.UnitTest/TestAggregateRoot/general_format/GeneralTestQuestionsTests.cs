using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using SharedKernel.Common.general_test_questions;
using TestCreationService.Domain.GeneralTestQuestionAggregate.events;
using TestCreationService.Domain.Rules;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Domain.UnitTests.TestAggregateRoot.general_format;

public class GeneralTestQuestionsTests
{
    private readonly AppUserId _creatorId = new AppUserId(Guid.NewGuid());

    private GeneralFormatTest CreateTestWithQuestions(int questionCount) {
        var test = GeneralFormatTest.CreateNew(_creatorId, "Test Name", new HashSet<AppUserId>())
            .GetSuccess();

        for (int i = 0; i < questionCount; i++) {
            test.AddNewQuestion(GeneralTestAnswersType.TextOnly);
        }

        return test;
    }

    [Fact]
    public void AddNewQuestion_ShouldAddQuestionSuccessfully_WhenUnderLimit() {
        // Arrange
        var test = CreateTestWithQuestions(GeneralFormatTestRules.MaxQuestionsCount - 1);

        // Act
        var result = test.AddNewQuestion(GeneralTestAnswersType.TextOnly);

        // Assert
        Assert.False(result.IsErr(), "Expected question to be added successfully.");
        Assert.Equal(GeneralFormatTestRules.MaxQuestionsCount, test.GetTestQuestionIds().Count);

        var domainEvents = test.GetDomainEventsCopy();
        Assert.Contains(domainEvents, e =>
            e is NewGeneralTestQuestionAddedEvent ev &&
            ev.AnswersType == GeneralTestAnswersType.TextOnly);
    }

    [Fact]
    public void AddNewQuestion_ShouldReturnError_WhenExceedingMaxQuestions() {
        // Arrange
        var test = CreateTestWithQuestions(GeneralFormatTestRules.MaxQuestionsCount);

        // Act
        var result = test.AddNewQuestion(GeneralTestAnswersType.TextOnly);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal($"General format test cannot have more than {GeneralFormatTestRules.MaxQuestionsCount} questions", err.Message);
    }

    [Fact]
    public void DeleteQuestion_ShouldDeleteQuestionSuccessfully_WhenQuestionExists() {
        // Arrange
        var test = CreateTestWithQuestions(5);
        var questionId = test.GetTestQuestionIds().First();

        // Act
        var result = test.DeleteQuestion(questionId);

        // Assert
        Assert.False(result.IsErr(), "Expected question to be deleted successfully.");
        Assert.DoesNotContain(questionId, test.GetTestQuestionIds());
        Assert.Equal(test.GetTestQuestionIds().Count, 4);

        // Проверка события
        var domainEvents = test.GetDomainEventsCopy();
        Assert.Contains(domainEvents, e =>
            e is GeneralTestQuestionDeletedEvent ev &&
            ev.QuestionId == questionId);
    }

    [Fact]
    public void DeleteQuestion_ShouldReturnError_WhenQuestionDoesNotExist() {
        // Arrange
        var test = CreateTestWithQuestions(1);
        var nonExistentQuestionId = GeneralTestQuestionId.CreateNew();

        // Act
        var result = test.DeleteQuestion(nonExistentQuestionId);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal(err.Code, Err.ErrCodes.NotFound);
    }
}