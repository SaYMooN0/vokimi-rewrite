using TestTakingService.Api.Contracts.load_test_taking_data_shared;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Api.Contracts.general_format_test.load_test_taking_data;

public record class GeneralTestTakingDataResponse(
    GeneralTestTakingQuestionData[] Questions,
    GeneralTestTakingFeedbackData Feedback,
    TestTakingStylesData Styles
)
{
    public static GeneralTestTakingDataResponse FromTest(GeneralFormatTest test) => new(
        test.Questions.Select(GeneralTestTakingQuestionData.FromQuestion).ToArray(),
        GeneralTestTakingFeedbackData.FromFeedbackOption(test.FeedbackOption ),
        TestTakingStylesData.FromStyles(test.Styles)
    );
}