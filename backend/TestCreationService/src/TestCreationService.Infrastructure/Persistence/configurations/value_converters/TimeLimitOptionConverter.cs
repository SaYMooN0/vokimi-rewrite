using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.tests.value_objects;

namespace TestCreationService.Infrastructure.Persistence.configurations.value_converters;

internal class TimeLimitOptionConverter : ValueConverter<TimeLimitOption, int>
{
    public TimeLimitOptionConverter() : base(
        v => v.TimeLimitExists ? (int)v.MaxSeconds : -1,
        v => v == -1
            ? TimeLimitOption.NoTimeLimit()
            : new TimeLimitOption(true, (ushort)v)
    ) {
    }
}
