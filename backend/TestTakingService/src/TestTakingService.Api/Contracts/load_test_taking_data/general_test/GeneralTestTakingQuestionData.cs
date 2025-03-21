using SharedKernel.Common.general_test_questions;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Api.Contracts.load_test_taking_data.general_test;

public record class GeneralTestTakingQuestionData(
    string Id,
    ushort OrderInTest,
    string Text,
    string[] Images,
    GeneralTestAnswersType AnswersType,
    GeneralTestTakingAnswerData[] Answers,
    bool ShuffleAnswers,
    ushort MinAnswersCount,
    ushort MaxAnswersCount,
    bool TimeLimitEnabled,
    ushort? TimeLimitSeconds
)
{
    public static GeneralTestTakingQuestionData FromQuestion(GeneralTestQuestion question) => new(
        question.Id.ToString(),
        question.OrderInTest,
        question.Text,
        question.Images.ToArray(),
        question.AnswersType,
        question.Answers.Select(GeneralTestTakingAnswerData.FromAnswer).ToArray(),
        question.ShuffleAnswers,
        question.AnswersCountLimit.MinAnswers,
        question.AnswersCountLimit.MaxAnswers,
        question.TimeLimit.TimeLimitExists,
        question.TimeLimit.Seconds
    );
}