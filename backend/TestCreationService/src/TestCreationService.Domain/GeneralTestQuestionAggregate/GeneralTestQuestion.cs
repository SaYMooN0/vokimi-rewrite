using OneOf.Types;
using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.general_test_questions;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;
using SharedKernel.IntegrationEvents.test_publishing;
using System.Collections.Immutable;
using TestCreationService.Domain.Common;
using TestCreationService.Domain.Rules;
using TestCreationService.Domain.TestAggregate.general_format.events;
using static System.Net.Mime.MediaTypeNames;

namespace TestCreationService.Domain.GeneralTestQuestionAggregate;

public class GeneralTestQuestion : AggregateRoot<GeneralTestQuestionId>
{
    private GeneralTestQuestion() { }
    public TestId TestId { get; init; }
    public string Text { get; private set; }
    public string[] _images { get; private set; } = [];
    public IReadOnlyList<string> Images => _images.AsReadOnly();
    public GeneralTestQuestionTimeLimitOption TimeLimit { get; private set; }
    public GeneralTestAnswersType AnswersType { get; init; }
    protected virtual List<GeneralTestAnswer> _answers { get; init; }
    private EntitiesOrderController<GeneralTestAnswerId> _answersOrderController { get; set; }
    public GeneralTestQuestionAnswersCountLimit AnswersCountLimit { get; private set; }
    public GeneralTestQuestion(GeneralTestQuestionId id, TestId testId, GeneralTestAnswersType answersType) {
        Id = id;
        TestId = testId;
        Text = "New question";
        _images = [];
        TimeLimit = GeneralTestQuestionTimeLimitOption.NoTimeLimit();
        AnswersType = answersType;
        _answers = new();
        _answersOrderController = EntitiesOrderController<GeneralTestAnswerId>.Empty(isShuffled: false);
        AnswersCountLimit = GeneralTestQuestionAnswersCountLimit.SingleChoice();
    }
    public ErrListOrNothing Update(
        string text,
        string[] images,
        GeneralTestQuestionTimeLimitOption timeLimit,
        GeneralTestQuestionAnswersCountLimit answerCountLimit
    ) {
        ErrList errs = new();
        errs.AddPossibleErr(GeneralTestQuestionRules.CheckQuestionTextForErrs(text));
        errs.AddPossibleErr(GeneralTestQuestionRules.CheckQuestionImagesForErrs(images));
        if (answerCountLimit.IsMultipleChoice && answerCountLimit.MaxAnswers > GeneralTestQuestionRules.MaxAnswersCount) {
            errs.Add(Err.ErrFactory.InvalidData(
                $"Multiple choice question cannot have more than {GeneralTestQuestionRules.MaxAnswersCount} answers, because it is the maximum count of answers that question can have")
            );
        }
        if (errs.Any()) { return errs; }
        Text = text;
        _images = images;
        TimeLimit = timeLimit;
        AnswersCountLimit = answerCountLimit;
        return ErrListOrNothing.Nothing;
    }
    public IReadOnlyList<(GeneralTestAnswer Question, ushort Order)> GetAnswersWithOrder() =>
        _answersOrderController.GetItemsWithOrders(_answers);
    public ErrOrNothing AddNewAnswer(
        GeneralTestAnswerTypeSpecificData answerData,
        HashSet<GeneralTestResultId> relatedResultIds
    ) {
        if (answerData.MatchingEnumType != AnswersType) {
            return new Err(
                "Unable to add new answer. Incorrect answer data provided",
                details: $"Question has answers type {AnswersType} and provided data is of type {answerData.MatchingEnumType}"
            );
        }
        if (_answers.Count >= GeneralTestQuestionRules.MaxAnswersCount) {
            return new Err(
                $"Question in the general format test cannot have more than {GeneralTestQuestionRules.MaxAnswersCount} answers",
                details: $"Maximum number of answers allowed is {GeneralTestQuestionRules.MaxAnswersCount}. Question already has {_answers.Count} answers"
            );
        }
        var creatingRes = GeneralTestAnswer.CreateNew(Id, answerData, relatedResultIds);
        if (creatingRes.IsErr(out var creatingErr)) {
            return creatingErr;
        }
        var newAnswer = creatingRes.GetSuccess();
        _answers.Add(newAnswer);
        _answersOrderController.AddToEnd(newAnswer.Id);
        _domainEvents.Add(new RelatedResultsForGeneralTestAnswerChangedEvent(TestId, relatedResultIds));
        return ErrOrNothing.Nothing;
    }
    public ErrOrNothing UpdateAnswer(
        GeneralTestAnswerId answerId,
        GeneralTestAnswerTypeSpecificData answerData,
        HashSet<GeneralTestResultId> relatedResultIds
    ) {
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
        var updateRes = answerToUpdate.Update(answerData, relatedResultIds);
        if (updateRes.IsErr(out var err)) {
            return err;
        }
        _domainEvents.Add(new RelatedResultsForGeneralTestAnswerChangedEvent(TestId, relatedResultIds));
        return ErrOrNothing.Nothing;
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
    public ErrOrNothing RemoveAnswer(GeneralTestAnswerId answerId) {
        var answer = _answers.FirstOrDefault(q => q.Id == answerId);
        if (answer is null) {
            return Err.ErrFactory.NotFound(
                message: $"Cannot find the answer",
                details: $"General test question with id {Id} has no answer with id {answerId}. Either it has already been removed or it was never in the question"
            );
        }
        _answers.Remove(answer);
        _answersOrderController.RemoveEntity(answer.Id);
        return ErrOrNothing.Nothing;
    }
    public void RemoveRelatedResultFromAnswers(GeneralTestResultId resultId) {
        foreach (var a in _answers) {
            a.RemoveRelatedResultId(resultId);
        }
    }
    public ImmutableHashSet<GeneralTestResultId> GetIdsOfResultsAnswersLeadTo() => _answers
        .SelectMany(a => a.GetRelatedResultIds())
        .ToImmutableHashSet();
    public IEnumerable<Err> CheckForPublishingProblems() {
        if (GeneralTestQuestionRules.CheckQuestionTextForErrs(Text).IsErr(out var err)) {
            yield return err;
        }
        if (GeneralTestQuestionRules.CheckQuestionImagesForErrs(_images).IsErr(out err)) {
            yield return err;
        }
        if (_answers is null) {
            yield return new Err(
                "Answers were not loaded",
                details: "Try again later. If it doesn't help try adding or removing one answer",
                source: ErrorSource.Server
            );
            yield break;
        }
        int answersCount = _answers.Count;
        if (answersCount > GeneralTestQuestionRules.MaxAnswersCount) {
            yield return new Err(
                $"Test question cannot have more than {GeneralTestQuestionRules.MaxAnswersCount} answers. Question has {answersCount} answers"
            );
            if (answersCount > GeneralTestQuestionRules.MaxAnswersCount * 2) {
                yield break;
            }
        }
        if (answersCount < GeneralTestQuestionRules.MinAnswersCount) {
            yield return new Err(
                $"Test question cannot have less than {GeneralTestQuestionRules.MinAnswersCount} answers. Question has {answersCount} answers"
            );
        }
        if (!AnswersCountLimit.IsMultipleChoice) {
            yield break;
        }
        if (answersCount < AnswersCountLimit.MaxAnswers) {
            yield return new Err(
              $"Question is a multiple choice question and with maximum number of answers to chose equal to {AnswersCountLimit.MaxAnswers}. Question has only {answersCount} answers"
            );
        }
        if (answersCount < AnswersCountLimit.MinAnswers) {
            yield return new Err(
              $"Question is a multiple choice question and with minimum number of answers to chose equal to {AnswersCountLimit.MinAnswers}. Question has only {answersCount} answers"
            );
        }
    }
    public GeneralTestPublishedQuestionDto ToTestPublishedDto(ushort order) => new(
        Id,
        order,
        Text,
        _images,
        TimeLimit,
        AnswersType,
        _answersOrderController.IsShuffled,
        AnswersCountLimit,
        _answers.Select(a=> new GeneralTestPublishedAnswerDto(
            a.Id, 
            a.TypeSpecificData, 
            a.GetRelatedResultIds().ToArray()
        )).ToArray()
    );
}
