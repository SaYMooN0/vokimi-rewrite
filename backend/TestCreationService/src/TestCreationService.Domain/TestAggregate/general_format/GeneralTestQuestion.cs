using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format_tests;
using SharedKernel.Common.tests.general_format_tests.answer_type_specific_data;
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
    private EntitiesOrderController<GeneralTestAnswerId> _answersOrderController { get; set; }
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
    public ErrOrNothing AddNewAnswer(GeneralTestAnswerTypeSpecificData answerData) {
        if (answerData.MatchingEnumType != AnswersType) {
            return new Err(
                "Unable to add new answer. Incorrect answer data provided",
                details: $"Question has answers type {AnswersType} and provided data is of type {answerData.MatchingEnumType}"
            );
        }
        if (_answers.Count >= GeneralFormatTestRules.MaxAnswersCount) {
            return new Err(
                $"Question in the general format test cannot have more than {GeneralFormatTestRules.MaxAnswersCount} answers",
                details: $"Maximum number of answers allowed is {GeneralFormatTestRules.MaxAnswersCount}. Question already has {_answers.Count} answers"
            );
        }
        var newAnswer = GeneralTestAnswer.CreateNew(Id, answerData);
        _answers.Add(newAnswer);
        _answersOrderController.AddToEnd(newAnswer);
        return ErrOrNothing.Nothing;
    }
    public ErrOrNothing UpdateAnswer(GeneralTestAnswerId answerId, GeneralTestAnswerTypeSpecificData answerData) {
        if (answerData.MatchingEnumType != AnswersType) {
            return new Err(
                "Unable to update the answer. Incorrect answer data provided",
                details: $"Question has answers type {AnswersType} and provided data is of type {answerData.MatchingEnumType}"
            );
        }
        var answerToUpdate = _answers.FirstOrDefault(a => a.Id == answerId);
        if (answerToUpdate is null) {
            return new Err(
                "Unable to update the answer. Cannot find the answer for the test",
                details: $"No answer with id {answerId} found in the question with id {Id}"
            );
        }
        return answerToUpdate.Update(answerData);
    }
    public ErrOrNothing UpdateAnswerOrder(EntitiesOrderController<GeneralTestAnswerId> orderController) {
        var answerIds = _answers.Select(q => q.Id).ToHashSet();
        var providedIds = orderController.EntityIds().ToHashSet();

        if (!answerIds.IsSubsetOf(providedIds)) {
            var missingIds = answerIds.Except(providedIds);
            return Err.ErrFactory.InvalidData(
                "Invalid answers order was provided. Not all answers presented",
                details: $"Missing answer Ids: {string.Join(", ", missingIds)}"
            );
        }

        var extraIds = providedIds.Except(answerIds);
        _answersOrderController = orderController.WithoutEntityIds(extraIds);
        return ErrOrNothing.Nothing;
    }
}
