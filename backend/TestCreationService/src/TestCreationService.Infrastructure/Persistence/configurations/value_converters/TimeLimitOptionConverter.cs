using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.tests.value_objects;

namespace TestCreationService.Infrastructure.Persistence.configurations.value_converters;

internal class TimeLimitOptionConverter : ValueConverter<TimeLimitOption, int>
{
    public TimeLimitOptionConverter() : base(
        v => v.ToInt(),
        v => FromInt(v)
    ) { }
    private static TimeLimitOption FromInt(int value) {
        var res = TimeLimitOption.FromInt(value);
        if (res.IsErr(out var err)) {
            throw new ArgumentException($"Incorrect time limit value in the data base: {err}");
        }
        return res.GetSuccess();
    }
}
