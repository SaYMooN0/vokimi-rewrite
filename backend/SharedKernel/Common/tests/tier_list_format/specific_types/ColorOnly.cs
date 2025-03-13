using System.Text.Json.Serialization;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects;

namespace SharedKernel.Common.tests.tier_list_format;

public abstract partial class TierListTestItemContentData
{
    public sealed class ColorOnly : TierListTestItemContentData
    {
        public HexColor Color { get; }
        private ColorOnly(HexColor color) => Color = color;

        [JsonIgnore] public override TierListTestItemContentType MatchingEnumType => TierListTestItemContentType.Color;

        public override IEnumerable<object> GetEqualityComponents() {
            yield return Color;
        }

        public override Dictionary<string, string> ToDictionary() =>
            new() { ["Color"] = Color.ToString() };

        public static ErrOr<ColorOnly> CreateFromDictionary(Dictionary<string, string> dictionary) {
            if (!dictionary.TryGetValue("Color", out string color)) {
                return Err.ErrFactory.InvalidData("Unable to create item content. Color not provided");
            }

            var parsedColor = HexColor.FromString(color);
            if (parsedColor.IsErr(out var colorErr)) {
                return colorErr;
            }

            return new ColorOnly(parsedColor.GetSuccess());
        }
    }
}