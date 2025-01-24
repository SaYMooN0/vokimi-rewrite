using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format_tests;
using SharedKernel.Common.tests.value_objects.answers_count_limit;
using SharedKernel.Common.tests.value_objects.time_limit_option;
using TestCreationService.Domain.Common;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Domain.TestAggregate.general_format;

public class GeneralTestQuestion : Entity<GeneralTestQuestionId>
{
    public TestId TestId { get; init; }
    public string Text { get; private set; }
    public string[] _images { get; private set; } = [];
    public IReadOnlyList<string> Images => _images.AsReadOnly();
    public GeneralTestQuestionTimeLimitOption TimeLimit { get; private set; }
    public GeneralTestAnswersType AnswersType { get; init; }
    protected virtual List<GeneralTestAnswer> _answers { get; init; }
    private EntitiesOrderController<GeneralTestAnswerId> _answersOrderController { get; init; }
    public GeneralTestQuestionAnswersCountLimit AnswerCountLimit { get; private set; }

    public static GeneralTestQuestion CreateNew(TestId testId, GeneralTestAnswersType answersType) => new() {
        Id = GeneralTestQuestionId.CreateNew(),
        Text = "New question",
        _images = [],
        TimeLimit = GeneralTestQuestionTimeLimitOption.NoTimeLimit(),
        AnswersType = answersType,
        _answers = new(),
        _answersOrderController = EntitiesOrderController<GeneralTestAnswerId>.Empty(isShuffled: false),
        AnswerCountLimit = GeneralTestQuestionAnswersCountLimit.SingleChoice(),
    };
    public ErrListOrNothing Update(
        string text,
        string[] images,
        GeneralTestQuestionTimeLimitOption timeLimit,
        GeneralTestQuestionAnswersCountLimit answerCountLimit
    ) {
        ErrList errs = new();
        errs.AddPossibleErr(GeneralFormatTestRules.CheckQuestionTextForErrs(text));
        errs.AddPossibleErr(GeneralFormatTestRules.CheckQuestionImagesForErrs(images));
        if (answerCountLimit.IsMultipleChoice && answerCountLimit.MaxAnswers > GeneralFormatTestRules.MaxAnswersCount) {
            errs.Add(Err.ErrFactory.InvalidData(
                $"Multiple choice question cannot have more than {GeneralFormatTestRules.MaxAnswersCount} answers, because it is the maximum count of answers that question can have")
            );
        }
        if (errs.Any()) {
            return errs;
        }
        Text = text;
        _images = images;
        TimeLimit = timeLimit;
        AnswerCountLimit = answerCountLimit;
        return ErrListOrNothing.Nothing;
    }
    public IReadOnlyList<(GeneralTestAnswer Question, ushort Order)> GetAnswersWithOrder() =>
        _answersOrderController.GetItemsWithOrders(_answers);
    public ErrOrNothing AddNewAnswer() {
        return Err.ErrFactory.NotImplemented();
    }
}
