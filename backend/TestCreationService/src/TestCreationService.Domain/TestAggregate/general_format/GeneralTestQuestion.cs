using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format_tests;
using SharedKernel.Common.tests.value_objects.answers_count_limit;
using SharedKernel.Common.tests.value_objects.time_limit_option;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Domain.TestAggregate.general_format;

public class GeneralTestQuestion : Entity
{
    protected override EntityId EntityId => Id;

    public GeneralTestQuestionId Id { get; init; }
    public TestId TestId { get; init; }
    public string Text { get; init; }
    private List<string> _images { get; init; } = [];
    public IReadOnlyList<string> Images => _images.AsReadOnly();
    public GeneralTestQuestionTimeLimitOption TimeLimit { get; private set; }
    public GeneralTestAnswersType AnswersType { get; init; }
    protected virtual List<GeneralTestAnswer> _answers { get; set; } = [];
    public IReadOnlyList<GeneralTestAnswer> Answers => _answers
        .OrderBy(a => _answersOrderDictionary.TryGetValue(a.Id, out var order) ? order : ushort.MaxValue)
        .ToList()
        .AsReadOnly();
    private Dictionary<GeneralTestAnswerId, ushort> _answersOrderDictionary { get; set; } = [];
    public GeneralTestQuestionAnswersCountLimit AnswerCountLimit { get; private set; }

    public static GeneralTestQuestion CreateNew(TestId testId, GeneralTestAnswersType answersType) => new() {
        Id = GeneralTestQuestionId.CreateNew(),
        Text = "New question",
        _images = [],
        TimeLimit = GeneralTestQuestionTimeLimitOption.NoTimeLimit(),
        AnswersType = answersType,
        _answers = [],
        _answersOrderDictionary = [],
        AnswerCountLimit = GeneralTestQuestionAnswersCountLimit.SingleChoice(),
    };
    public static ErrListOrNothing Update(
        string text,
        string[] images,
        GeneralTestQuestionTimeLimitOption timeLimit,
        GeneralTestQuestionAnswersCountLimit answerCountLimit
    ) {
        ErrList errs = new();
        errs.AddPossibleErr(GeneralFormatTestRules.CheckQuestionTextForErrs(text));
        errs.AddPossibleErr(GeneralFormatTestRules.CheckQuestionImagesForErrs(images));
        if(answerCountLimit.IsMultipleChoice && answerCountLimit.MaxAnswers> GeneralFormatTestRules.MaxAnswersCount) {
            errs.Add(Err.ErrFactory.InvalidData(
                $"Multiple choice question cannot have more than {GeneralFormatTestRules.MaxAnswersCount} answers, because it is the maximum count of answers that question can have")
            );
        }
        return errs;

    }
}
