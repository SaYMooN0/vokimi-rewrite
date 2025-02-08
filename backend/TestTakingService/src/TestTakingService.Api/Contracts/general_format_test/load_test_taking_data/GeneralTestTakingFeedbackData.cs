using SharedKernel.Common.common_enums;
using SharedKernel.Common.tests.general_format;

namespace TestTakingService.Api.Contracts.general_format_test.load_test_taking_data;

public record class GeneralTestTakingFeedbackData(
    bool IsEnabled,
    AnonymityValues? Anonymity,
    string? AccompanyingText,
    ushort? MaxFeedbackLength
)
{
    public static GeneralTestTakingFeedbackData FromFeedbackOption(GeneralTestFeedbackOption feedback) =>
        feedback switch {
            GeneralTestFeedbackOption.Disabled => new(
                IsEnabled: false,
                Anonymity: null,
                AccompanyingText: null,
                MaxFeedbackLength: null
            ),
            GeneralTestFeedbackOption.Enabled enabled => new(
                IsEnabled: true,
                Anonymity: enabled.Anonymity,
                AccompanyingText: enabled.AccompanyingText,
                MaxFeedbackLength: enabled.MaxFeedbackLength
            ),
            _ => throw new ArgumentException("Unknown feedback option type", nameof(feedback))
        };
}