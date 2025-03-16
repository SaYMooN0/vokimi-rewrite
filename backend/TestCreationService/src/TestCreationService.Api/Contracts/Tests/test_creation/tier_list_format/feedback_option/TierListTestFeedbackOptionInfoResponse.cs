using SharedKernel.Common.common_enums;
using SharedKernel.Common.tests.tier_list_format.feedback;

namespace TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.feedback_option;

public record class TierListTestFeedbackOptionInfoResponse(
    bool EnableTestFeedback,
    AnonymityValues? FeedbackAnonymity,
    string? AccompanyingText,
    ushort? MaxFeedbackLength
)
{
    public static TierListTestFeedbackOptionInfoResponse FromFeedbackOption(
        TierListTestFeedbackOption option
    ) => option is TierListTestFeedbackOption.Enabled enabled
        ? new TierListTestFeedbackOptionInfoResponse(
            true, enabled.Anonymity, enabled.AccompanyingText, enabled.MaxFeedbackLength
        )
        : new TierListTestFeedbackOptionInfoResponse(
            false, null, null, null
        );
}