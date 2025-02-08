using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Api.Contracts.general_format_test.load_test_taking_data;

public record GeneralTestTakingAnswerData(
    string Id,
    ushort OrderInQuestion,
    Dictionary<string, string> TypeSpecificData
)
{
    public static GeneralTestTakingAnswerData FromAnswer(GeneralTestAnswer answer) => new(
        answer.Id.ToString(),
        answer.OrderInQuestion,
        answer.TypeSpecificData.ToDictionary()
    );
}