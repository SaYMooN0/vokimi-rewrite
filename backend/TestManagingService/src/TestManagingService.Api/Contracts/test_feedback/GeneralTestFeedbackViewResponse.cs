using SharedKernel.Common.common_enums;
using SharedKernel.Common.tests.general_format;

namespace TestManagingService.Api.Contracts.test_feedback;

public record class GeneralTestFeedbackViewResponse(
    bool IsEnabled,
    AnonymityValues Anonymity,
    string AccompanyingText,
    ushort MaxFeedbackLength
)
{
    public static GeneralTestFeedbackViewResponse FromFeedbackOption(
        GeneralTestFeedbackOption feedbackOption
    ) => feedbackOption switch {
        GeneralTestFeedbackOption.Enabled enabled => new GeneralTestFeedbackViewResponse(
            true, enabled.Anonymity, enabled.AccompanyingText, enabled.MaxFeedbackLength
        ),
        GeneralTestFeedbackOption.Disabled disabled => new GeneralTestFeedbackViewResponse(
            false, AnonymityValues.Any, "", 0
        ),
        _ => throw new ArgumentOutOfRangeException(
            $"Uknkown {nameof(GeneralTestFeedbackOption)} value of {nameof(feedbackOption)}"
        )
    };
}