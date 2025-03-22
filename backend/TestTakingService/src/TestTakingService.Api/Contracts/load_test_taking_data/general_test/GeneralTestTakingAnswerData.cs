using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Api.Contracts.load_test_taking_data.general_test;

internal record GeneralTestTakingAnswerData(
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