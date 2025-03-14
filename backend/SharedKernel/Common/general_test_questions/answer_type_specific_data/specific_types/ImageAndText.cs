using System.Text.Json.Serialization;
using SharedKernel.Common.errors;

namespace SharedKernel.Common.general_test_questions.answer_type_specific_data;

public abstract partial class GeneralTestAnswerTypeSpecificData
{
    public sealed class ImageAndText : GeneralTestAnswerTypeSpecificData
    {
        public string Image { get; }
        public string Text { get; }

        private ImageAndText(string image, string text) {
            Image = image;
            Text = text;
        }

        [JsonIgnore] public override GeneralTestAnswersType MatchingEnumType => GeneralTestAnswersType.ImageAndText;

        public override IEnumerable<object> GetEqualityComponents() {
            yield return Image;
            yield return Text;
        }

        public override Dictionary<string, string> ToDictionary() => new() {
            ["Image"] = Image,
            ["Text"] = Text
        };

        public static ErrOr<ImageAndText> CreateFromDictionary(Dictionary<string, string> dictionary) {
            if (!dictionary.TryGetValue("Text", out string text)) {
                return Err.ErrFactory.InvalidData("Unable to create type specific data. Text not provided");
            }

            if (!dictionary.TryGetValue("Image", out string image)) {
                return Err.ErrFactory.InvalidData("Unable to create type specific data. Image not provided");
            }

            return CreateNew(image, text);
        }

        public static ErrOr<ImageAndText> CreateNew(string image, string text) {
            if (!GeneralTestAnswerTypeSpecificDataRules.IsStringCorrectAnswerText(text, out int textLength)) {
                return Err.ErrFactory.InvalidData(
                    $"Answer text must be between {GeneralTestAnswerTypeSpecificDataRules.AnswerMinLength} and {GeneralTestAnswerTypeSpecificDataRules.AnswerMaxLength} characters",
                    details: $"Current length: {textLength}"
                );
            }

            if (!GeneralTestAnswerTypeSpecificDataRules.IsStringCorrectNonTextItem(image, out int imageLength)) {
                return Err.ErrFactory.InvalidData(
                    $"Image data must be non-empty and at most {GeneralTestAnswerTypeSpecificDataRules.NonTextDataMaxLength} characters",
                    details: $"Current length: {imageLength}"
                );
            }

            return new ImageAndText(image, text);
        }
    }
}