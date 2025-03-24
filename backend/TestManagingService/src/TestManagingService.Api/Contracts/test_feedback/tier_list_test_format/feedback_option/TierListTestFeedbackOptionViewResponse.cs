using SharedKernel.Common.common_enums;
using SharedKernel.Common.tests.tier_list_format.feedback;

namespace TestManagingService.Api.Contracts.test_feedback.tier_list_test_format.feedback_option;

public record class TierListTestFeedbackOptionViewResponse(
    bool IsEnabled,
    AnonymityValues Anonymity,
    string AccompanyingText,
    ushort MaxFeedbackLength
)
{
    public static TierListTestFeedbackOptionViewResponse FromFeedbackOption(
        TierListTestFeedbackOption feedbackOption
    ) => feedbackOption switch {
        TierListTestFeedbackOption.Enabled enabled => new TierListTestFeedbackOptionViewResponse(
            true, enabled.Anonymity, enabled.AccompanyingText, enabled.MaxFeedbackLength
        ),
        TierListTestFeedbackOption.Disabled disabled => new TierListTestFeedbackOptionViewResponse(
            false, AnonymityValues.Any, "", 0
        ),
        _ => throw new ArgumentOutOfRangeException(
            $"Uknkown {nameof(TierListTestFeedbackOption)} value of {nameof(feedbackOption)}"
        )
    };
}