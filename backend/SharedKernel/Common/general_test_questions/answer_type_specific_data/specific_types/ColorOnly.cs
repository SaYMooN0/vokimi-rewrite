using System.Text.Json.Serialization;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects;

namespace SharedKernel.Common.general_test_questions.answer_type_specific_data;
public abstract partial class GeneralTestAnswerTypeSpecificData
{
    public sealed class ColorOnly : GeneralTestAnswerTypeSpecificData
    {
        private ColorOnly() { }
        public HexColor Color { get; init; } = null!;
        [JsonIgnore]
        public override GeneralTestAnswersType MatchingEnumType => GeneralTestAnswersType.ColorOnly;

        public static ErrOr<ColorOnly> CreateNew(HexColor color) {
            return new ColorOnly { Color = color };
        }

        public override IEnumerable<object> GetEqualityComponents() {
            yield return Color;
        }
        public override Dictionary<string, string> ToDictionary() => new Dictionary<string, string> {
            ["Color"] = Color.ToString(),
        };

        public static ErrOr<ColorOnly> CreateFromDictionary(Dictionary<string, string> dictionary) {
            if (!dictionary.TryGetValue("Color", out string color)) {
                return Err.ErrFactory.InvalidData("Unable to create type specific data. Color not provided");
            }

            var parsedColor = HexColor.FromString(color);

            if (parsedColor.IsErr(out var colorErr)) {
                return colorErr;
            }

            return CreateNew(parsedColor.GetSuccess());
        }

    }
}