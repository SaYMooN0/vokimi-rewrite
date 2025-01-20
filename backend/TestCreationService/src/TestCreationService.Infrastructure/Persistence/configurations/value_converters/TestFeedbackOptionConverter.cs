using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Infrastructure.Persistence.configurations.value_converters;

internal class TestFeedbackOptionConverter : ValueConverter<TestFeedbackOption, string>
{
    public TestFeedbackOptionConverter() : base(
        v => ToString(v),
        v => FromString(v)
    ) { }
    private static string ToString(TestFeedbackOption option) {
        return option switch {
            TestFeedbackOption.Disabled => "Disabled",
            TestFeedbackOption.Enabled enabled => JsonSerializer.Serialize(enabled),
            _ => throw new InvalidOperationException($"Unknown TestFeedbackOption type: {option.GetType()}")
        };
    }

    private static TestFeedbackOption FromString(string value) {
        if (value == "Disabled") { return TestFeedbackOption.Disabled.Instance; }

        return JsonSerializer.Deserialize<TestFeedbackOption.Enabled>(value)
                ?? throw new FormatException("Invalid format for TestFeedbackOption.");
    }
}
