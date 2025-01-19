using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
using SharedKernel.Common.tests.general_format_tests;
using System.Collections.Immutable;
using TestCreationService.Domain.Common.rules;
using TestCreationService.Domain.TestAggregate.formats_shared;
using TestCreationService.Domain.TestAggregate.formats_shared.events;

namespace TestCreationService.Domain.TestAggregate.general_format;

public class GeneralFormatTest : BaseTest
{
    private GeneralFormatTest() { }
    public override TestFormat Format => TestFormat.General;

    protected virtual List<GeneralTestQuestion> _questions { get; init; } = [];
    public IReadOnlyList<GeneralTestQuestion> Questions => _questions
        .OrderBy(q => _questionsOrderDictionary.TryGetValue(q.Id, out var order) ? order : ushort.MaxValue)
        .ToList()
        .AsReadOnly();
    private Dictionary<GeneralTestQuestionId, ushort> _questionsOrderDictionary = [];
    protected virtual List<GeneralTestResult> _results { get; init; } = [];
    public IReadOnlyList<GeneralTestResult> Results => _results
        .OrderBy(r => r.Id)
        .ToList()
        .AsReadOnly();

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
    ) : base(TestId.CreateNew(), creatorId, editorIds, mainInfo, TestSettings.Deafult, TestStyles.Default) { }

    public ErrOrNothing AddNewQuestion(GeneralTestAnswersType answersType) {
        if (Questions.Count >= GeneralFormatTestRules.MaxQuestionsCount) {
            return new Err(
                $"General format test cannot have more than {GeneralFormatTestRules.MaxQuestionsCount} questions",
                details: $"Maximum number of questions allowed is {GeneralFormatTestRules.MaxQuestionsCount}. Test already has {Questions.Count} questions."
            );
        }
        var newQuestion = GeneralTestQuestion.CreateNew(Id, answersType);
        _questions.Add(newQuestion);
        _questionsOrderDictionary.Add(newQuestion.Id, (ushort)(_questionsOrderDictionary.Count + 1));
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
        RemoveQuestionFromOrderDictionary(question.Id);
        return ErrOrNothing.Nothing;
    }
    private void RemoveQuestionFromOrderDictionary(GeneralTestQuestionId questionId) {
        if (_questionsOrderDictionary.TryGetValue(questionId, out var removedOrder)) {
            foreach (var (id, order) in _questionsOrderDictionary) {
                if (removedOrder < order) {
                    _questionsOrderDictionary[id]--;
                }
            }
            _questionsOrderDictionary.Remove(questionId);
        }
    }
    public IImmutableDictionary<ushort, GeneralTestQuestion> GetAllQuestionsWithCorrectOrder() {
        ushort currentOrder = 1;
        Dictionary<ushort, GeneralTestQuestion> orderedQuestions = new(_questions.Count);

        foreach (var question in Questions) {
            orderedQuestions[currentOrder] = question;
            currentOrder++;
        }

        return orderedQuestions.ToImmutableDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

}
