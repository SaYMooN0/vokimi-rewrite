using System.Text.Json.Serialization;
using SharedKernel.Common.errors;

namespace SharedKernel.Common.tests.tier_list_format.items;


public abstract partial class TierListTestItemContentData
{
    public sealed class ImageOnly : TierListTestItemContentData
    {
        public string Image { get; }

        private ImageOnly(string image) => Image = image;

        [JsonIgnore]
        public override TierListTestItemContentType MatchingEnumType =>
            TierListTestItemContentType.ImageOnly;

        public override IEnumerable<object> GetEqualityComponents() {
            yield return Image;
        }

        public override Dictionary<string, string> ToDictionary() =>
            new() { ["Image"] = Image };

        public static ErrOr<ImageOnly> CreateFromDictionary(Dictionary<string, string> dictionary) {
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

            return new ImageOnly(image);
        }
    }
}