using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using SharedKernel.Common.tests.general_format;
using TestTakingService.Domain.Common.general_test_taken_data;

namespace TestTakingService.Domain.TestAggregate.general_format.helpers;

internal static class GeneralTestTakingValidatorHelper
{
    internal static ErrOrNothing ValidatePossibleFeedbackForTestTakenRequest(
        AppUserId? testTakerId,
        GeneralTestTakenFeedbackData? feedbackData,
        GeneralTestFeedbackOption testFeedbackOption
    ) {
        if (feedbackData is null) {
            return ErrOrNothing.Nothing;
        }

        if (testFeedbackOption is GeneralTestFeedbackOption.Disabled) {
            return new Err("Feedback for this test is disabled");
        }

        if (testTakerId is null && feedbackData.LeftAnonymously == false) {
            return new Err(
                "The feedback was left non-anonymously, but the user ID is not provided",
                details: "Ensure that you are logged in"
            );
        }

        var thisFeedbackOption = testFeedbackOption as GeneralTestFeedbackOption.Enabled;
        if (thisFeedbackOption is null) {
            return new Err("Invalid feedback option configuration");
        }

        if (ValidateFeedbackAnonymity(feedbackData, thisFeedbackOption.Anonymity).IsErr(out var err)) {
            return err;
        }

        if (ValidateFeedbackLength(feedbackData, thisFeedbackOption).IsErr(out err)) {
            return err;
        }

        return ErrOrNothing.Nothing;
    }

    private static ErrOrNothing ValidateFeedbackAnonymity(
        GeneralTestTakenFeedbackData feedback,
        AnonymityValues thisFeedbackOptionAnonymity
    ) => (feedback.LeftAnonymously, thisFeedbackOptionAnonymity) switch {
        (LeftAnonymously: true, AnonymityValues.NonAnonymousOnly) => new Err(
            "You are trying to leave anonymous feedback, but this test only supports non-anonymous feedback"
        ),
        (LeftAnonymously: false, AnonymityValues.AnonymousOnly) => new Err(
            "You are trying to leave non-anonymous feedback, but this test only supports anonymous feedback"
        ),
        _ => ErrOrNothing.Nothing
    };

    private static ErrOrNothing ValidateFeedbackLength(
        GeneralTestTakenFeedbackData feedback,
        GeneralTestFeedbackOption.Enabled enabledFeedbackOption
    ) {
        int feedbackTextLen = feedback.FeedbackText?.Length ?? 0;
        if (feedbackTextLen == 0) {
            return new Err("You are trying to leave feedback, but it's empty");
        }

        if (feedbackTextLen > enabledFeedbackOption.MaxFeedbackLength) {
            return new Err(
                $"Your feedback text is too long. Maximum length of feedback is {enabledFeedbackOption.MaxFeedbackLength}",
                details: $"Current feedback text is {feedbackTextLen} characters"
            );
        }

        return ErrOrNothing.Nothing;
    }

    internal static ErrOr<GeneralTestTakenQuestionData> GetValidQuestionData(
        Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> questionsDataMap,
        GeneralTestQuestion question
    ) {
        if (!questionsDataMap.TryGetValue(question.Id, out var questionData)) {
            return new Err(
                "Answers for not all questions are chosen",
                details: $"Question with id '{question.Id}' has no answers chosen"
            );
        }

        if (!question.TimeLimit.TimeLimitExists) {
            if (CheckChosenAnswersCount(question, questionData).IsErr(out var err)) {
                return err;
            }
        }


        HashSet<GeneralTestAnswerId> questionAnswerIds = question.Answers.Select(a => a.Id).ToHashSet();
        var incorrectAnswers = questionData.ChosenAnswers
            .Where(a => !questionAnswerIds.Contains(a));

        if (incorrectAnswers.Any()) {
            var incorrectAnswerIdsString = string.Join(", ", incorrectAnswers.Select(a => a.ToString()));
            return new Err(
                "Question has some answers marked as chosen that do not belong to this question",
                details:
                $"Question id: {question.Id}, incorrect answers: {incorrectAnswerIdsString}"
            );
        }

        return questionData;
    }

    private static ErrOrNothing CheckChosenAnswersCount(
        GeneralTestQuestion question,
        GeneralTestTakenQuestionData questionData
    ) {
        int chosenAnswersCount = questionData.ChosenAnswers.Count;

        if (question.AnswersCountLimit.MinAnswers > chosenAnswersCount) {
            return new Err(
                $"Question has {chosenAnswersCount} answers chosen but must have at least {question.AnswersCountLimit.MinAnswers} ",
                details:
                $"Question id: {question.Id}, minimum answers count: {question.AnswersCountLimit.MinAnswers}"
            );
        }

        if (question.AnswersCountLimit.MaxAnswers < chosenAnswersCount) {
            return new Err(
                $"Question has {chosenAnswersCount} answers chosen but must have at most {question.AnswersCountLimit.MaxAnswers} ",
                details:
                $"Question id: {question.Id}, maximum answers count: {question.AnswersCountLimit.MaxAnswers}"
            );
        }

        return ErrOrNothing.Nothing;
    }

    internal static ErrOrNothing CheckForTestTimeDurationErrs(
        IDateTimeProvider dateTimeProvider,
        IEnumerable<TimeSpan> timeSpentOnQuestions,
        DateTime testStartTime,
        DateTime testEndTime
    ) {
        if (testStartTime > testEndTime) {
            return new Err("Test start time cannot be later than end time");
        }

        if (testStartTime > dateTimeProvider.Now) {
            return new Err("Test couldn't start in a future");
        }

        double totalTimeSpentOnQuestions = timeSpentOnQuestions.Sum(q => q.TotalSeconds);
        double testDuration = (testEndTime - testStartTime).TotalSeconds;

        if (totalTimeSpentOnQuestions > testDuration) {
            return new Err("Somehow total time spent on questions exceeds the total test duration");
        }

        return ErrOrNothing.Nothing;
    }
}