using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using SharedKernel.Common.general_test_questions;
using SharedKernel.Common.interfaces;
using SharedKernel.Common.tests;
using SharedKernel.Common.tests.general_format;
using SharedKernel.IntegrationEvents.test_publishing;
using System.Collections.Immutable;
using TestCreationService.Domain.Common;
using TestCreationService.Domain.GeneralTestQuestionAggregate;
using TestCreationService.Domain.GeneralTestQuestionAggregate.events;
using TestCreationService.Domain.Rules;
using TestCreationService.Domain.TestAggregate.formats_shared;
using TestCreationService.Domain.TestAggregate.formats_shared.events;
using TestCreationService.Domain.TestAggregate.general_format.events;

namespace TestCreationService.Domain.TestAggregate.general_format;

public class GeneralFormatTest : BaseTest
{
    private GeneralFormatTest() { }
    public override TestFormat Format => TestFormat.General;


    protected EntitiesOrderController<GeneralTestQuestionId> _questionsList { get; set; }
    protected virtual List<GeneralTestResult> _results { get; init; }

    public ImmutableArray<GeneralTestResult> Results => _results
        .OrderBy(r => r.Id)
        .ToImmutableArray();

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

        editorIds.Remove(creatorId);
        if (editorIds.Count() > TestRules.MaxTestEditorsCount) {
            return new Err(
                message: "Too many test editors",
                details: $"You can't add more than {TestRules.MaxTestEditorsCount} editors to the test. Current count of unique editors: {editorIds.Count()}"
            );
        }
        var newTest = new GeneralFormatTest(
            creatorId,
            editorIds.ToHashSet(),
            mainInfoCreation.GetSuccess()
        );
        newTest._domainEvents.Add(new TestEditorsListChangedEvent(newTest.Id, editorIds, new HashSet<AppUserId>()));
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
                details:
                $"Maximum number of questions allowed is {GeneralFormatTestRules.MaxQuestionsCount}. Test already has {_questionsList.Count} questions"
            );
        }

        GeneralTestQuestionId questionId = GeneralTestQuestionId.CreateNew();
        _questionsList.AddToEnd(questionId);
        _domainEvents.Add(new NewGeneralTestQuestionAddedEvent(Id, questionId, answersType));
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing DeleteQuestion(GeneralTestQuestionId questionId) {
        if (!_questionsList.Contains(questionId)) {
            return Err.ErrFactory.NotFound(
                "There is no such question in this test",
                details: $"Test with id {Id} does not have question with id {questionId}"
            );
        }

        _questionsList.RemoveEntity(questionId);
        _domainEvents.Add(new GeneralTestQuestionDeletedEvent(questionId));
        return ErrOrNothing.Nothing;
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

    public bool HasResultWithId(GeneralTestResultId resultId) =>
        _results.Any(r => r.Id == resultId);

    public ImmutableDictionary<GeneralTestResultId, string> GetTestResultIdsWithNames() =>
        _results.ToImmutableDictionary(r => r.Id, r => r.Name);

    public ErrOr<GeneralTestResult> CreateResult() {
        if (_results.Count >= GeneralFormatTestRules.MaxResultsInTestCount) {
            return new Err(
                $"General format test cannot have more than {GeneralFormatTestRules.MaxResultsInTestCount} results",
                details:
                $"Maximum number of results allowed is {GeneralFormatTestRules.MaxResultsInTestCount}. Test already has {_results.Count}"
            );
        }

        string resultName = "Test result #" + (_results.Count + 1);
        var creationRes = GeneralTestResult.CreateNew(Id, resultName);
        if (creationRes.IsErr(out var err)) {
            return err;
        }

        _results.Add(creationRes.GetSuccess());
        return creationRes.GetSuccess();
    }

    public ErrOrNothing DeleteResult(GeneralTestResultId resultId) {
        GeneralTestResult? result = _results.FirstOrDefault(r => r.Id == resultId);
        if (result is null) {
            return Err.ErrFactory.NotFound(
                "There is no such result in this test",
                details: $"General format test with id {Id} doesn't have result with id {resultId}"
            );
        }

        _results.Remove(result);
        _domainEvents.Add(new GeneralTestResultDeletedEvent(Id, result.Id));
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing UpdateResult(
        GeneralTestResultId resultId,
        string Name,
        string Text,
        string Image
    ) {
        GeneralTestResult? result = _results.FirstOrDefault(r => r.Id == resultId);
        if (result is null) {
            return Err.ErrFactory.NotFound(
                "Cannot update result for general format test. There is no such result in this test",
                details: $"General format test with id {Id} doesn't have result with id {resultId}"
            );
        }

        GeneralTestResult? resultWithSameName = _results.FirstOrDefault(r => r.Name == Name && r.Id != resultId);
        if (resultWithSameName is not null) {
            return Err.ErrFactory.InvalidData(
                "Cannot update result. Test already has result with such name. Result name must be unique",
                details:
                $"General format test result with id {resultWithSameName.Id} already has name {resultWithSameName.Name}"
            );
        }

        return result.Update(Name, Text, Image);
    }

    public List<TestPublishingProblem> CheckForPublishingProblems(IEnumerable<GeneralTestQuestion> questions) => [
        .. _mainInfo.CheckForPublishingProblems().Select(e => TestPublishingProblem.FromErr(e, "Main Info")),
        .. CheckQuestionsForPublishingProblems(questions).Select(e => TestPublishingProblem.FromErr(e, "Questions")),
        .. CheckResultsForPublishingProblems(questions).Select(e => TestPublishingProblem.FromErr(e, "Results")),
        .. _tags.CheckForPublishingProblems().Select(e => TestPublishingProblem.FromErr(e, "Tags")),
    ];

    private IEnumerable<Err> CheckQuestionsForPublishingProblems(IEnumerable<GeneralTestQuestion> questions) {
        if (questions is null) {
            yield return new Err(
                "Questions were not loaded",
                details: "Try again later. If it doesn't help try adding or removing one question",
                source: ErrorSource.Server
            );
            yield break;
        }

        int questionsCount = questions.Count();
        if (questionsCount > GeneralFormatTestRules.MaxQuestionsCount) {
            yield return new Err(
                $"General format test cannot have more than {GeneralFormatTestRules.MaxQuestionsCount} questions. Test has {questionsCount} questions"
            );
            if (questionsCount > GeneralFormatTestRules.MaxQuestionsCount * 2) {
                yield break;
            }
        }

        if (questionsCount < GeneralFormatTestRules.MinQuestionsCount) {
            yield return new Err(
                $"General format test must have at least {GeneralFormatTestRules.MinQuestionsCount} questions. Test has {questionsCount} questions"
            );
        }

        foreach (var question in questions) {
            var errs = question.CheckForPublishingProblems();
            if (errs.Any()) {
                string prefix = $"Question" +
                                (string.IsNullOrWhiteSpace(question.Text)
                                    ? $"with id: {question.Id}"
                                    : $"'{question.Text.Take(20)}...'");
                foreach (var err in errs) {
                    yield return err.WithPrefix(prefix);
                }
            }
        }
    }

    private IEnumerable<Err> CheckResultsForPublishingProblems(IEnumerable<GeneralTestQuestion> questions) {
        if (_results is null) {
            yield return new Err(
                "Results were not loaded",
                details: "Try again later. If it doesn't help try adding or removing one result",
                source: ErrorSource.Server
            );
            yield break;
        }

        int resultsCount = _results.Count;
        if (resultsCount > GeneralFormatTestRules.MaxResultsInTestCount) {
            yield return new Err(
                $"General format test cannot have more than {GeneralFormatTestRules.MaxResultsInTestCount} results. Test has {resultsCount} results"
            );
            if (resultsCount > GeneralFormatTestRules.MaxResultsInTestCount * 2) {
                yield break;
            }
        }

        if (resultsCount < GeneralFormatTestRules.MinResultsInTestCount) {
            yield return new Err(
                $"General format test must have at least {GeneralFormatTestRules.MinResultsInTestCount} results. Test has {resultsCount} results"
            );
        }

        var resultIdsAnswersLeadTo =
            questions is null ? [] : questions.SelectMany(q => q.GetIdsOfResultsAnswersLeadTo()).ToHashSet();
        foreach (var result in _results) {
            if (!resultIdsAnswersLeadTo.Contains(result.Id)) {
                yield return new Err(
                    $"Result '{result.Name}' has no answers leading to it. Result must have at least one answer leading to it");
            }
        }
    }

    public ErrOrNothing Publish(IEnumerable<GeneralTestQuestion> questions, IDateTimeProvider dateTimeProvider) {
        if (CheckForPublishingProblems(questions).Any()) {
            return new Err("Cannot publish test. Test has publishing problems");
        }

        GeneralTestPublishedEvent e = new(
            Id, CreatorId, EditorIds.ToArray(), _mainInfo.Name, _mainInfo.CoverImg, _mainInfo.Description,
            _mainInfo.Language,
            dateTimeProvider.Now,
            new(
                _interactionsAccessSettings.TestAccess,
                _interactionsAccessSettings.AllowRatings,
                _interactionsAccessSettings.AllowDiscussions,
                _interactionsAccessSettings.AllowTestTakenPosts,
                _interactionsAccessSettings.AllowTagsSuggestions
            ),
            new(
                _styles.AccentColor,
                _styles.ErrorsColor,
                _styles.Buttons
            ),
            _tags.GetTags().ToArray(),
            new(
                TestTakingProcessSettings.ForceSequentialFlow,
                TestTakingProcessSettings.Feedback
            ),
            _questionsList.IsShuffled,
            _questionsList
                .GetItemsWithOrders(questions)
                .Select(i => i.Entity.ToTestPublishedDto(i.Order))
                .ToArray(),
            _results.Select(r => new GeneralTestPublishedResultDto(r.Id, r.Name, r.Text, r.Image)).ToArray()
        );
        return ErrOrNothing.Nothing;
    }

    public override void DeleteTest() {
        foreach (var questionId in _questionsList.EntityIds()) {
            this.DeleteQuestion(questionId);
        }

        _domainEvents.Add(new GeneralFormatTestDeletedEvent(Id, CreatorId, _editorIds));
    }
}