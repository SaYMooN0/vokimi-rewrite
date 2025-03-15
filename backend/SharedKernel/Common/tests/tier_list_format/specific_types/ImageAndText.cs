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

            if (TierListTestItemRules.CheckIfStringCorrectItemTextContent(text).IsErr(out var textErr)) {
                return textErr;
            }

            if (!dictionary.TryGetValue("Image", out string image)) {
                return Err.ErrFactory.InvalidData("Unable to create item content. Image not provided");
            }

            if (
                TierListTestItemRules
                .CheckIdStringCorrectRequiredNonTextItemContent(image, "Image")
                .IsErr(out var err)
            ) {
                return err;
            }

            return new ImageAndText(image, text);
        }
    }
}