using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.general_test_questions;
using SharedKernel.Common.tests;
using System.Collections.Immutable;
using TestCreationService.Domain.Common;
using TestCreationService.Domain.GeneralTestQuestionAggregate;
using TestCreationService.Domain.GeneralTestQuestionAggregate.events;
using TestCreationService.Domain.Rules;
using TestCreationService.Domain.TestAggregate.formats_shared;
using TestCreationService.Domain.TestAggregate.formats_shared.events;

namespace TestCreationService.Domain.TestAggregate.general_format;

public class GeneralFormatTest : BaseTest
{
    private GeneralFormatTest() { }
    public override TestFormat Format => TestFormat.General;
    protected EntitiesOrderController<GeneralTestQuestionId> _questionsList { get; set; }
    protected virtual List<GeneralTestResult> _results { get; init; }
    public IReadOnlyList<GeneralTestResult> Results => _results.OrderBy(r => r.Id).ToImmutableList();
    private GeneralTestTakingProcessSettings TestTakingProcessSettings { get; init; }

    public static ErrOr<GeneralFormatTest> CreateNew(
        AppUserId creatorId,
        string testName,
        HashSet<AppUserId> editorIds
    ) {
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
        _questionsList = EntitiesOrderController<GeneralTestQuestionId>.Empty(isShuffled: false);
        _results = [];
        TestTakingProcessSettings = GeneralTestTakingProcessSettings.CreateNew();
    }

    public ErrOrNothing AddNewQuestion(GeneralTestAnswersType answersType) {
        if (_questionsList.Count >= GeneralFormatTestRules.MaxQuestionsCount) {
            return new Err(
                $"General format test cannot have more than {GeneralFormatTestRules.MaxQuestionsCount} questions",
                details: $"Maximum number of questions allowed is {GeneralFormatTestRules.MaxQuestionsCount}. Test already has {_questionsList.Count} questions"
            );
        }
        GeneralTestQuestionId questionId = GeneralTestQuestionId.CreateNew();
        _questionsList.AddToEnd(questionId);
        _domainEvents.Add(new NewGeneralTestQuestionAddedEvent(questionId, answersType));
        return ErrOrNothing.Nothing;
    }
    public void DeleteQuestion(GeneralTestQuestionId questionId) {
        _questionsList.RemoveEntity(questionId);
    }
    public void UpdateTestTakingProcessSettings(
        bool forceSequentialFlow,
        GeneralTestFeedbackOption testFeedback
    ) => TestTakingProcessSettings.Update(forceSequentialFlow, testFeedback);
    public ErrOrNothing UpdateQuestionsOrder(EntitiesOrderController<GeneralTestQuestionId> orderController) {
        var questionIds = _questionsList.EntityIds();
        var providedIds = orderController.EntityIds().ToHashSet();

        if (!questionIds.IsSubsetOf(providedIds)) {
            var missingIds = questionIds.Except(providedIds);
            return Err.ErrFactory.InvalidData(
                "Invalid questions order was provided. Not all questions presented",
                details: $"Missing question Ids: {string.Join(", ", missingIds)}"
            );
        }

        var extraIds = providedIds.Except(questionIds);
        _questionsList = orderController.WithoutEntityIds(extraIds);
        return ErrOrNothing.Nothing;
    }
    public ImmutableHashSet<GeneralTestQuestionId> GetTestQuestionIds() =>
        _questionsList.EntityIds();
    public ErrOr<ImmutableArray<(GeneralTestQuestion Question, ushort Order)>> GetQuestionsWithOrder(
        IEnumerable<GeneralTestQuestion> questions
    ) {
        if (questions.Any(q => !_questionsList.Contains(q.Id))) {
            return new Err(
                "General format test doesn't have information about order for all questions",
                details: "Try again later. If it doesn't help try adding or removing one question",
                source: ErrorSource.Server
            );
        }
        return _questionsList.GetItemsWithOrders(questions);
    }
}
