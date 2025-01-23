using SharedKernel.Common.common_enums;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
using SharedKernel.Common.tests.general_format_tests;
using System.Collections.Immutable;
using TestCreationService.Domain.Common;
using TestCreationService.Domain.Rules;
using TestCreationService.Domain.TestAggregate.formats_shared;
using TestCreationService.Domain.TestAggregate.formats_shared.events;

namespace TestCreationService.Domain.TestAggregate.general_format;

public class GeneralFormatTest : BaseTest
{
    private GeneralFormatTest() { }
    public override TestFormat Format => TestFormat.General;

    protected virtual List<GeneralTestQuestion> _questions { get; init; }
    protected EntitiesOrderController<GeneralTestQuestionId> _questionsOrderController { get; set; }
    protected virtual List<GeneralTestResult> _results { get; init; }
    public IReadOnlyList<GeneralTestResult> Results => _results.OrderBy(r => r.Id).ToImmutableList();
    private GeneralTestTakingProcessSettings TestTakingProcessSettings { get; init; }

    public static ErrOr<GeneralFormatTest> CreateNew(AppUserId creatorId, string testName, HashSet<AppUserId> editorIds) {
        var mainInfoCreation = TestMainInfo.CreateNew(testName);
        if (mainInfoCreation.IsErr(out var err)) {
            return err;
        }
        if (editorIds.Contains(creatorId)) { editorIds.Remove(creatorId); }

        var newTest = new GeneralFormatTest(
            creatorId,
            editorIds.ToHashSet(),
            mainInfoCreation.GetSuccess()
        );
        newTest._domainEvents.Add(new TestEditorsListChangedEvent(newTest.Id, editorIds, []));
        newTest._domainEvents.Add(new NewTestInitializedEvent(newTest.Id, newTest.CreatorId));
        return newTest;
    }
    private GeneralFormatTest(
        AppUserId creatorId,
        HashSet<AppUserId> editorIds,
        TestMainInfo mainInfo
    ) : base(TestId.CreateNew(), creatorId, editorIds, mainInfo) {
        _questions = [];
        _questionsOrderController = EntitiesOrderController<GeneralTestQuestionId>.Empty(isShuffled: false);
        _results = [];
        TestTakingProcessSettings = GeneralTestTakingProcessSettings.CreateNew();
    }

    public ErrOrNothing AddNewQuestion(GeneralTestAnswersType answersType) {
        if (_questions.Count >= GeneralFormatTestRules.MaxQuestionsCount) {
            return new Err(
                $"General format test cannot have more than {GeneralFormatTestRules.MaxQuestionsCount} questions",
                details: $"Maximum number of questions allowed is {GeneralFormatTestRules.MaxQuestionsCount}. Test already has {_questions.Count} questions."
            );
        }
        var newQuestion = GeneralTestQuestion.CreateNew(Id, answersType);
        _questions.Add(newQuestion);
        _questionsOrderController.AddToEnd(newQuestion);
        return ErrOrNothing.Nothing;
    }
    public ErrOrNothing RemoveQuestion(GeneralTestQuestionId questionId) {
        var question = _questions.FirstOrDefault(q => q.Id == questionId);
        if (question is null) {
            return Err.ErrFactory.NotFound(
                message: $"Question with id {questionId} was not found in this test",
                details: $"General format test with id {Id} has no question with id {questionId}. Either it has already been removed or it was never in the test"
            );
        }
        _questions.Remove(question);
        _questionsOrderController.RemoveEntity(question);
        return ErrOrNothing.Nothing;
    }
    public IReadOnlyList<(GeneralTestQuestion Question, ushort Order)> GetQuestionsWithOrder() =>
        _questionsOrderController.GetItemsWithOrders(_questions);
    public void UpdateTestTakingProcessSettings(
        bool forceSequentialFlow,
        GeneralTestFeedbackOption testFeedback
    ) => TestTakingProcessSettings.Update(forceSequentialFlow, testFeedback);
    public ErrOrNothing UpdateQuestionsOrder(EntitiesOrderController<GeneralTestQuestionId> orderController) {
        var questionIds = _questions.Select(q => q.Id).ToHashSet();
        var providedIds = orderController.EntityIds().ToHashSet();

        if (!questionIds.IsSubsetOf(providedIds)) {
            var missingIds = questionIds.Except(providedIds);
            return Err.ErrFactory.InvalidData(
                "Invalid questions order was provided. Not all questions presented",
                details: $"Missing question Ids: {string.Join(", ", missingIds)}"
            );
        }

        var extraIds = providedIds.Except(questionIds);
        _questionsOrderController = orderController.WithoutEntityIds(extraIds);
        return ErrOrNothing.Nothing;
    }
}
