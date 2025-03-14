using System.Text.Json.Serialization;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects;

namespace SharedKernel.Common.general_test_questions.answer_type_specific_data;

public abstract partial class GeneralTestAnswerTypeSpecificData
{
    public sealed class ColorOnly : GeneralTestAnswerTypeSpecificData
    {
        public HexColor Color { get; }
        private ColorOnly(HexColor color) => Color = color;
        [JsonIgnore] public override GeneralTestAnswersType MatchingEnumType => GeneralTestAnswersType.ColorOnly;

        public override IEnumerable<object> GetEqualityComponents() {
            yield return Color;
        }

        public override Dictionary<string, string> ToDictionary() =>
            new() { ["Color"] = Color.ToString() };

        public static ErrOr<ColorOnly> CreateFromDictionary(Dictionary<string, string> dictionary) {
            if (!dictionary.TryGetValue("Color", out string color)) {
                return Err.ErrFactory.InvalidData("Unable to create type specific data. Color not provided");
            }

            return CreateNew(color);
        }

        public static ErrOr<ColorOnly> CreateNew(string color) {
            var parsedColor = HexColor.FromString(color);
            if (parsedColor.IsErr(out var colorErr)) {
                return colorErr;
            }

            return new ColorOnly(parsedColor.GetSuccess());
        }
    }
}