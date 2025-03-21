using TestTakingService.Api.Contracts.test_taken.test_formats_shared;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Api.Contracts.load_test_taking_data.general_test;

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