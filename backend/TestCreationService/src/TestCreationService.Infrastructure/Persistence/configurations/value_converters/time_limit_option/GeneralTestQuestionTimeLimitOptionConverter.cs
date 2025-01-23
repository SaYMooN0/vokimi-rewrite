using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.tests.value_objects.time_limit_option;

namespace TestCreationService.Infrastructure.Persistence.configurations.value_converters;

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
