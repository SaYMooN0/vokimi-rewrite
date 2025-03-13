using System.Text.Json.Serialization;
using SharedKernel.Common.errors;

namespace SharedKernel.Common.tests.tier_list_format;

public abstract partial class TierListTestItemContentData
{
    public sealed class ImageAndText : TierListTestItemContentData
    {
        public string Image { get; }
        public string Text { get; }

        private ImageAndText(string image, string text) {
            Image = image;
            Text = text;
        }
        [JsonIgnore]
        public override TierListTestItemContentType MatchingEnumType => TierListTestItemContentType.ImageAndText;

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
                return Err.ErrFactory.InvalidData("Unable to create item content. Text not provided");
            }

            if (!TierListTestItemContentTypeSpecificDataRules.IsStringCorrectItemText(text, out int textLength)) {
                return Err.ErrFactory.InvalidData(
                    $"Answer text must be between {TierListTestItemContentTypeSpecificDataRules.ItemTextMinLength} and {TierListTestItemContentTypeSpecificDataRules.ItemTextMaxLength} characters",
                    details: $"Current length: {textLength}"
                );
            }

            if (!dictionary.TryGetValue("Image", out string image)) {
                return Err.ErrFactory.InvalidData("Unable to create item content. Image not provided");
            }

            if (!TierListTestItemContentTypeSpecificDataRules.IsStringCorrectNonTextItem(image, out int imageLength)) {
                return Err.ErrFactory.InvalidData(
                    $"Image data must be non-empty and at most {TierListTestItemContentTypeSpecificDataRules.NonTextDataMaxLength} characters",
                    details: $"Current length: {imageLength}"
                );
            }

            return new ImageAndText(image, text);
        }
    }
}
