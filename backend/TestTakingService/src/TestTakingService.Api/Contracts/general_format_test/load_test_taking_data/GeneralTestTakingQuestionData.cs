using SharedKernel.Common.general_test_questions;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Api.Contracts.general_format_test.load_test_taking_data;

public record class GeneralTestTakingQuestionData(
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

