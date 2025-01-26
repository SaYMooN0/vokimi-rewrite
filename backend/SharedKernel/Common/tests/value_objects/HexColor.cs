using SharedKernel.Common.errors;

namespace SharedKernel.Common.tests.value_objects;

public class HexColor : ValueObject
{
    private HexColor() { }

    public string Value { get; init; } = null!;

    public static ErrOr<HexColor> FromString(string colorValue) {
        colorValue = colorValue.Trim();

        if (!IsValidHexColor(colorValue)) {
            return Err.ErrFactory.InvalidData($"Invalid hex color format: {colorValue}");
        }

        if (!colorValue.StartsWith("#")) {
            colorValue = "#" + colorValue;
        }

        return new HexColor { Value = colorValue };
    }

    public static bool IsValidHexColor(string colorValue) {
        if (colorValue.StartsWith("#")) {
            colorValue = colorValue.Substring(1);
        }

        return System.Text.RegularExpressions.Regex.IsMatch(colorValue, "^([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$");
    }

    public override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }

    public override string ToString() => Value;
}