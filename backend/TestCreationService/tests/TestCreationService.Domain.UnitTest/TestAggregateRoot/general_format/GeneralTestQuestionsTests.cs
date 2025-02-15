using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.errors;
using SharedKernel.Common.general_test_questions;
using TestCreationService.Domain.Common;
using TestCreationService.Domain.GeneralTestQuestionAggregate;
using TestCreationService.Domain.GeneralTestQuestionAggregate.events;
using TestCreationService.Domain.Rules;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Domain.UnitTests.TestAggregateRoot.general_format;

public class GeneralTestQuestionsTests
{
    private readonly AppUserId _creatorId = new(Guid.NewGuid());

    private GeneralFormatTest CreateTestWithQuestions(int questionCount) {
        var test = GeneralFormatTest.CreateNew(_creatorId, "Test Name", new HashSet<AppUserId>())
            .GetSuccess();

        for (var i = 0; i < questionCount; i++) {
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
        Assert.Equal($"General format test cannot have more than {GeneralFormatTestRules.MaxQuestionsCount} questions",
            err.Message);
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

    [Fact]
    public void UpdateQuestionsOrder_ShouldUpdateOrderSuccessfully_WhenValidOrderProvided() {
        // Arrange
        var test = CreateTestWithQuestions(3);

        var testQuestions = test.GetTestQuestionIds()
            .Select(qId => new GeneralTestQuestion(qId, test.Id, GeneralTestAnswersType.TextOnly))
            .ToList();

        var newQuestionIdWithOrder = test.GetQuestionsWithOrder(testQuestions)
            .GetSuccess()
            .ToDictionary(qwo => qwo.Question.Id, qwo => (ushort)(4 - qwo.Order));

        var orderController =
            EntitiesOrderController<GeneralTestQuestionId>.CreateNew(isShuffled: true, newQuestionIdWithOrder)
                .GetSuccess();

        // Act
        var result = test.UpdateQuestionsOrder(orderController);

        // Assert
        var updatedOrder = test
            .GetQuestionsWithOrder(testQuestions)
            .GetSuccess();

        foreach (var updated in updatedOrder) {
            var expectedOrder = newQuestionIdWithOrder[updated.Question.Id];
            Assert.Equal(updated.Order, expectedOrder);
        }
    }

    [Fact]
    public void UpdateQuestionsOrder_ShouldReturnError_WhenMissingQuestionsInOrder() {
        // Arrange
        var test = CreateTestWithQuestions(3);
        var initialOrder = test.GetTestQuestionIds().ToList();
        var newOrder = initialOrder.Take(2).ToList(); // Недостающий один вопрос

        var entityOrders = newOrder
            .Select(
                (id, index) => new { Id = id, Index = (ushort)(index + 1) }
            ).ToDictionary(o => o.Id, o => o.Index);

        var orderControllerResult = EntitiesOrderController<GeneralTestQuestionId>.CreateNew(true, entityOrders);

        // Act
        if (orderControllerResult.IsErr(out var err)) {
            Assert.Fail($"Expected success but got error: {err.Message}");
        }

        var orderController = orderControllerResult.GetSuccess();
        var result = test.UpdateQuestionsOrder(orderController);

        // Assert
        Assert.True(result.IsErr(out err), "Expected error due to missing questions.");
        Assert.Contains("Invalid questions order was provided", err.Message);
    }

    [Fact]
    public void UpdateQuestionsOrder_ShouldReturnError_WhenExtraQuestionsInOrder() {
        // Arrange
        var test = CreateTestWithQuestions(3);
        var initialOrder = test.GetTestQuestionIds().ToList();
        var newOrder =
            initialOrder.Concat([GeneralTestQuestionId.CreateNew()]).ToList();

        var entityOrders = newOrder
            .Select(
                (id, index) => new { Id = id, Index = (ushort)(index + 1) }
            ).ToDictionary(o => o.Id, o => o.Index);

        var orderControllerResult = EntitiesOrderController<GeneralTestQuestionId>.CreateNew(true, entityOrders);

        // Act
        if (orderControllerResult.IsErr(out var err)) {
            Assert.Fail($"Expected success but got error: {err.Message}");
        }

        var orderController = orderControllerResult.GetSuccess();
        var result = test.UpdateQuestionsOrder(orderController);

        // Assert
        Assert.True(result.IsErr(out err), "Expected error due to extra questions");
        Assert.Equal(err.Code, Err.ErrCodes.InvalidData);
    }

    [Fact]
    public void GetTestQuestionIds_ShouldReturnCorrectIds() {
        // Arrange
        var test = CreateTestWithQuestions(3);

        // Act
        var questionIds = test.GetTestQuestionIds();

        // Assert
        Assert.Equal(3, questionIds.Count);
    }

    [Fact]
    public void GetQuestionsWithOrder_ShouldReturnQuestionsWithCorrectOrder_WhenAllQuestionsPresent() {
        // Arrange
        var test = GeneralFormatTest.CreateNew(_creatorId, "Test Name", new HashSet<AppUserId>())
            .GetSuccess();

        for (var i = 0; i < 5; i++) {
            test.AddNewQuestion(GeneralTestAnswersType.TextOnly);
        }

        var questions = test.GetTestQuestionIds()
            .Select(qId => new GeneralTestQuestion(qId, test.Id, GeneralTestAnswersType.TextOnly))
            .ToList();

        // Act
        var result = test.GetQuestionsWithOrder(questions);

        // Assert
        Assert.True(result.IsSuccess(), "Expected questions with order.");
        var questionsWithOrder = result.GetSuccess();
        Assert.Equal(questions.Count, questionsWithOrder.Length);

        for (ushort i = 0; i < questionsWithOrder.Length; i++) {
            Assert.Equal((ushort)(i + 1), questionsWithOrder[i].Order);
        }
    }

    [Fact]
    public void GetQuestionsWithOrder_ShouldReturnError_WhenSomeQuestionsAreMissingFromTest() {
        // Arrange
        var test = CreateTestWithQuestions(3);
        var questions = new List<GeneralTestQuestion> {
            new(GeneralTestQuestionId.CreateNew(), test.Id, GeneralTestAnswersType.TextOnly)
        };

        // Act
        var result = test.GetQuestionsWithOrder(questions);

        // Assert
        Assert.True(result.IsErr(out var err), "Expected error when some questions are missing.");
        Assert.Contains("General format test doesn't have information about order for all questions", err.Message);
    }
}