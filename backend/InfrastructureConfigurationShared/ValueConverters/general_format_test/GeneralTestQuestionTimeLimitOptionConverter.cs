using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.general_test_questions;

namespace InfrastructureConfigurationShared.ValueConverters.general_format_test;

internal class GeneralTestQuestionTimeLimitOptionConverter : ValueConverter<GeneralTestQuestionTimeLimitOption, int>
{
    public GeneralTestQuestionTimeLimitOptionConverter() : base(
        v => v.ToInt(),
        v => FromInt(v)
    ) { }
    private static GeneralTestQuestionTimeLimitOption FromInt(int value) {
        var res = GeneralTestQuestionTimeLimitOption.FromInt(value);
        if (res.IsErr(out var err)) {
            throw new ArgumentException($"Incorrect time limit value in the data base: {err}");
        }
        return res.GetSuccess();
    }
}
