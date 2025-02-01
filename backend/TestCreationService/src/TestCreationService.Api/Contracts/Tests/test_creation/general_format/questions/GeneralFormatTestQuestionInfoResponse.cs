using TestCreationService.Domain.GeneralTestQuestionAggregate;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.questions;

internal record class GeneralFormatTestQuestionInfoResponse(
    string Id,
    string Text,
    string[] Images,
    int TimeLimit,
    string AnswersType,
    string AnswerCountLimit,
    ushort OrderInTest
)
{
    public static GeneralFormatTestQuestionInfoResponse FromQuestion(GeneralTestQuestion question, ushort questionOrder) => new(
        Id: question.Id.ToString(),
        Text: question.Text,
        Images: question.Images.ToArray(),
        TimeLimit: question.TimeLimit.ToInt(),
        AnswersType: question.AnswersType.ToString(),
        AnswerCountLimit: question.AnswersCountLimit.ToString(),
        OrderInTest: questionOrder
    );
}
