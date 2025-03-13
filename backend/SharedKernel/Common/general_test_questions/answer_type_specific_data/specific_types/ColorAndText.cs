using System.Text.Json.Serialization;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects;

namespace SharedKernel.Common.general_test_questions.answer_type_specific_data;
public abstract partial class GeneralTestAnswerTypeSpecificData
{
    public sealed class ColorAndText : GeneralTestAnswerTypeSpecificData
    {
        public HexColor Color { get; }
        public string Text { get;  }

        private ColorAndText(HexColor color, string text) {
            Color = color;
            Text = text;
        }
        [JsonIgnore]
        public override GeneralTestAnswersType MatchingEnumType => GeneralTestAnswersType.ColorAndText;

        public override IEnumerable<object> GetEqualityComponents() {
            yield return Color;
            yield return Text;
        }
        public override Dictionary<string, string> ToDictionary() => new() {
            ["Text"] = Text,
            ["Color"] = Color.ToString(),
        };

        public static ErrOr<ColorAndText> CreateFromDictionary(Dictionary<string, string> dictionary) {
            if (!dictionary.TryGetValue("Text", out string text)) {
                return Err.ErrFactory.InvalidData("Unable to create type specific data. Text not provided");
            }
            if (!GeneralTestAnswerTypeSpecificDataRules.IsStringCorrectAnswerText(text, out int textLength)) {
                return Err.ErrFactory.InvalidData(
                    $"Answer text must be between {GeneralTestAnswerTypeSpecificDataRules.AnswerMinLength} and {GeneralTestAnswerTypeSpecificDataRules.AnswerMaxLength} characters",
                    details: $"Current length: {textLength}"
                );
            }
            if (!dictionary.TryGetValue("Color", out string color)) {
                return Err.ErrFactory.InvalidData("Unable to create type specific data. Color not provided");
            }

            var parsedColor = HexColor.FromString(color);
            if (parsedColor.IsErr(out var colorErr)) {
                return colorErr;
            }

            return new ColorAndText(parsedColor.GetSuccess(), text);
        }
    }
}
