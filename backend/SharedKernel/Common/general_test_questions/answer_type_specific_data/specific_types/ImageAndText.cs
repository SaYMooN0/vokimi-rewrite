using SharedKernel.Common.errors;
using System.Text.Json.Serialization;

namespace SharedKernel.Common.general_test_questions.answer_type_specific_data;
public abstract partial class GeneralTestAnswerTypeSpecificData
{
    public sealed class ImageAndText : GeneralTestAnswerTypeSpecificData
    {
        private ImageAndText() { }
        public string Text { get; init; } = null!;
        public string Image { get; init; } = null!;
        [JsonIgnore] 
        public override GeneralTestAnswersType MatchingEnumType => GeneralTestAnswersType.ImageAndText;

        public static ErrOr<ImageAndText> CreateNew(string text, string image) {
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

            return new ImageAndText { Text = text, Image = image };
        }

        public override IEnumerable<object> GetEqualityComponents() {
            yield return Text;
            yield return Image;
        }
        public override Dictionary<string, string> ToDictionary() => new Dictionary<string, string> {
            ["Text"] = Text,
            ["Image"] = Image,
        };

        public static ErrOr<ImageAndText> CreateFromDictionary(Dictionary<string, string> dictionary) {
            if (!dictionary.TryGetValue("Text", out string text)) {
                return Err.ErrFactory.InvalidData("Unable to create type specific data. Text not provided");
            }
            if (!dictionary.TryGetValue("Image", out string image)) {
                return Err.ErrFactory.InvalidData("Unable to create type specific data. Image not provided");
            }
            return CreateNew(text, image);
        }

    }
}