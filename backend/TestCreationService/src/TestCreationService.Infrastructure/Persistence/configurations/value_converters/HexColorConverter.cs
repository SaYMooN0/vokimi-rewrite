using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.tests.value_objects;

namespace TestCreationService.Infrastructure.Persistence.configurations.value_converters;

internal class HexColorConverter : ValueConverter<HexColor, string>
{
    public HexColorConverter() : base(
        v => v.Value,
        v => HexColorFromString(v)
    ) {
    }
    private static HexColor HexColorFromString(string str) =>
        HexColor.FromString(str).Match(
            success => success,
            err => throw new ArgumentException($"Incorrect hex color in database: {str}")
        );
}