using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.tests.general_format;

namespace InfrastructureConfigurationShared.ValueConverters.general_format_test;

internal class GeneralTestFeedbackOptionConverter : ValueConverter<GeneralTestFeedbackOption, string>
{
    public GeneralTestFeedbackOptionConverter() : base(
        v => ToString(v),
        v => FromString(v)
    ) { }

    private static string ToString(GeneralTestFeedbackOption option) {
        return option switch {
            GeneralTestFeedbackOption.Disabled => "Disabled",
            GeneralTestFeedbackOption.Enabled enabled => JsonSerializer.Serialize(enabled),
            _ => throw new InvalidOperationException($"Unknown TestFeedbackOption type: {option.GetType()}")
        };
    }

    private static GeneralTestFeedbackOption FromString(string value) {
        if (value == "Disabled") {
            return GeneralTestFeedbackOption.Disabled.Instance;
        }

        return JsonSerializer.Deserialize<GeneralTestFeedbackOption.Enabled>(value)
               ?? throw new FormatException("Invalid format for TestFeedbackOption");
    }
}