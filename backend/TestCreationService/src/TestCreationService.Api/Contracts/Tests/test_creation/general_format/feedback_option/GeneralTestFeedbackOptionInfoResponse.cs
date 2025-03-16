using SharedKernel.Common.common_enums;
using SharedKernel.Common.tests.general_format;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.feedback_option;

public record class GeneralTestFeedbackOptionInfoResponse(
    bool EnableTestFeedback,
    AnonymityValues? FeedbackAnonymity,
    string? AccompanyingText,
    ushort? MaxFeedbackLength
)
{
    public static GeneralTestFeedbackOptionInfoResponse FromFeedbackOption(
        GeneralTestFeedbackOption option
    ) => option is GeneralTestFeedbackOption.Enabled enabled
        ? new GeneralTestFeedbackOptionInfoResponse(
            true, enabled.Anonymity, enabled.AccompanyingText, enabled.MaxFeedbackLength
        )
        : new GeneralTestFeedbackOptionInfoResponse(
            false, null, null, null
        );
}