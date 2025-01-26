using SharedKernel.Common.errors;
using System.Text.Json.Serialization;

namespace SharedKernel.Common.general_test_questions.answer_type_specific_data;
public abstract partial class GeneralTestAnswerTypeSpecificData
{
    public sealed class ImageOnly : GeneralTestAnswerTypeSpecificData
    {
        private ImageOnly() { }
        public string Image { get; init; } = null!;
        [JsonIgnore]
        public override GeneralTestAnswersType MatchingEnumType => GeneralTestAnswersType.ImageOnly;

        public static ErrOr<ImageOnly> CreateNew(string image) {
            if (!GeneralTestAnswerTypeSpecificDataRules.IsStringCorrectNonTextItem(image, out int imageLength)) {
                return Err.ErrFactory.InvalidData(
                    $"Image data must be non-empty and at most {GeneralTestAnswerTypeSpecificDataRules.NonTextDataMaxLength} characters",
                    details: $"Current length: {imageLength}"
                );
            }

            return new ImageOnly { Image = image };
        }

        public override IEnumerable<object> GetEqualityComponents() {
            yield return Image;
        }
        public override Dictionary<string, string> ToDictionary() => new Dictionary<string, string> {
            ["Image"] = Image,
        };

        public static ErrOr<ImageOnly> CreateFromDictionary(Dictionary<string, string> dictionary) {
            if (!dictionary.TryGetValue("Image", out string image)) {
                return Err.ErrFactory.InvalidData("Unable to create type specific data. Image not provided");
            }
            return CreateNew(image);
        }

    }
}