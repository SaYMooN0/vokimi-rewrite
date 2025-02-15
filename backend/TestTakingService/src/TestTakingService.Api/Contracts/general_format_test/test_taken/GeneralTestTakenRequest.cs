using ApiShared.interfaces;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.errors;
using TestTakingService.Domain.Common.general_test_taken_data;

namespace TestTakingService.Api.Contracts.general_format_test.test_taken;

internal record class GeneralTestTakenRequest(
    GeneralTestTakenRequestQuestionInfo[] Questions,
    GeneralTestTakenFeedbackData? Feedback,
    DateTime StartDateTime,
    DateTime EndDateTime
) : IRequestWithValidationNeeded
{
    private const int _maxQuestionsCount = 100;

    public RequestValidationResult Validate() {
        if (Questions is null || !Questions.Any()) {
            return new ErrList(Err.ErrFactory.InvalidData(
                "Info about question is not provided")
            );
        }

        if (Questions.Length > _maxQuestionsCount) {
            return new ErrList(Err.ErrFactory.InvalidData(
                "Too many question info provided")
            );
        }

        ErrList errs = new();

        foreach (var q in Questions) {
            errs.AddPossibleErr(q.CheckForErr());
        }

        if (Feedback is not null && (Feedback.FeedbackText?.Length ?? 0) == 0) {
            errs.Add(Err.ErrFactory.InvalidData("Seems like feedback should be provided, but its text is empty"));
        }

        if (StartDateTime >= EndDateTime) {
            errs.Add(Err.ErrFactory.InvalidData("Start date must be before end date"));
        }

        return errs;
    }

    public Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> ParsedQuestionInfo => Questions.ToDictionary(
        q => new GeneralTestQuestionId(Guid.Parse(q.QuestionId)),
        q => new GeneralTestTakenQuestionData(
            q.ChosenAnswerIds.Select(a => new GeneralTestAnswerId(new(a))).ToHashSet(),
            q.TimeSpentOnQuestion
        ));
}

internal record class GeneralTestTakenRequestQuestionInfo(
    string QuestionId,
    string[] ChosenAnswerIds,
    TimeSpan TimeSpentOnQuestion
)
{
    private const int _maxAnswersCount = 100;

    public ErrOrNothing CheckForErr() {
        if (!Guid.TryParse(QuestionId, out _)) {
            return Err.ErrFactory.InvalidData(
                "Invalid chosen answers entry. Incorrect question Id",
                $"Provided value: {QuestionId}"
            );
        }

        if (ChosenAnswerIds.Length > _maxAnswersCount) {
            return Err.ErrFactory.InvalidData(
                "Too many answers provided as chosen for the question",
                $"Question id: {QuestionId}"
            );
        }

        HashSet<string> uniqueAnswers = new();
        foreach (var aId in ChosenAnswerIds) {
            if (!Guid.TryParse(aId, out _)) {
                return Err.ErrFactory.InvalidData(
                    "Invalid chosen answers entry. Incorrect chosen answer Id",
                    $"Invalid answer id: {aId} for question with Id: {QuestionId}"
                );
                continue;
            }

            if (!uniqueAnswers.Add(aId)) {
                return Err.ErrFactory.InvalidData(
                    "Duplicate answer detected for the same question",
                    $"Duplicate answer id: {aId} for question with Id: {QuestionId}"
                );
            }
        }

        return ErrOrNothing.Nothing;
    }
}