using ApiShared.interfaces;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestTakingService.Domain.Common.test_taken_data.general_format_test;

namespace TestTakingService.Api.Contracts.test_taken.general_test;

internal class GeneralTestTakenRequest : TestTakenRequest
{
    public GeneralTestTakenRequestQuestionInfo[] Questions { get; init; } = [];
    public GeneralTestTakenFeedbackData? Feedback { get; init; }
    public override DateTime StartDateTime { get; init; }
    public override DateTime EndDateTime { get; init; }


    private const int _maxQuestionsCount = 100;

    public override RequestValidationResult Validate() {
        if (Questions.Length == 0) {
            return new ErrList(Err.ErrFactory.InvalidData(
                "Info about questions is not provided"
            ));
        }

        if (Questions.Length > _maxQuestionsCount) {
            return new ErrList(Err.ErrFactory.InvalidData(
                "Too many question info provided"
            ));
        }

        ErrList errs = new();
        errs.AddPossibleErr(base.ValidateStartAndEndDateTine());

        foreach (var q in Questions) {
            errs.AddPossibleErr(q.CheckForErr());
        }

        if (Feedback is not null && (Feedback.FeedbackText?.Length ?? 0) == 0) {
            errs.Add(Err.ErrFactory.InvalidData("Seems like feedback should be provided, but its text is empty"));
        }


        return errs;
    }

    public Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> ParsedQuestionInfo =>
        Questions.ToDictionary(
            q => new GeneralTestQuestionId(Guid.Parse(q.QuestionId)),
            q => new GeneralTestTakenQuestionData(
                q.ChosenAnswerIds.Select(a => new GeneralTestAnswerId(new(a))).ToHashSet(),
                q.TimeSpentOnQuestion
            )
        );
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