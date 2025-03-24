using SharedKernel.Common.common_enums;
using SharedKernel.Common.tests.general_format;

namespace TestManagingService.Api.Contracts.test_feedback.general_test_format.feedback_option;

public record class GeneralTestFeedbackOptionViewResponse(
    bool IsEnabled,
    AnonymityValues Anonymity,
    string AccompanyingText,
    ushort MaxFeedbackLength
)
{
    public static GeneralTestFeedbackOptionViewResponse FromFeedbackOption(
        GeneralTestFeedbackOption feedbackOption
    ) => feedbackOption switch {
        GeneralTestFeedbackOption.Enabled enabled => new GeneralTestFeedbackOptionViewResponse(
            true, enabled.Anonymity, enabled.AccompanyingText, enabled.MaxFeedbackLength
        ),
        GeneralTestFeedbackOption.Disabled disabled => new GeneralTestFeedbackOptionViewResponse(
            false, AnonymityValues.Any, "", 0
        ),
        _ => throw new ArgumentOutOfRangeException(
            $"Uknkown {nameof(GeneralTestFeedbackOption)} value of {nameof(feedbackOption)}"
        )
    };
}