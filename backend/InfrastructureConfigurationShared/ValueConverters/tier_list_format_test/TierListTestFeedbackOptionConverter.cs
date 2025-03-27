using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.tests.tier_list_format.feedback;

namespace InfrastructureConfigurationShared.ValueConverters.tier_list_format_test;

internal class TierListTestFeedbackOptionConverter : ValueConverter<TierListTestFeedbackOption, string>
{
    public TierListTestFeedbackOptionConverter() : base(
        v => ToString(v),
        v => FromString(v)
    ) { }

    private static string ToString(TierListTestFeedbackOption option) {
        return option switch {
            TierListTestFeedbackOption.Disabled => "Disabled",
            TierListTestFeedbackOption.Enabled enabled => JsonSerializer.Serialize(enabled),
            _ => throw new InvalidOperationException($"Unknown TestFeedbackOption type: {option.GetType()}")
        };
    }

    private static TierListTestFeedbackOption FromString(string value) {
        if (value == "Disabled") {
            return TierListTestFeedbackOption.Disabled.Instance;
        }

        return JsonSerializer.Deserialize<TierListTestFeedbackOption.Enabled>(value)
               ?? throw new FormatException("Invalid format for TestFeedbackOption");
    }
}