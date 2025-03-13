using System.Text.Json.Serialization;
using SharedKernel.Common.errors;

namespace SharedKernel.Common.tests.tier_list_format;

public abstract partial class TierListTestItemContentData
{
    public sealed class TextOnly : TierListTestItemContentData
    {
        public string Text { get; }

        private TextOnly(string text) => Text = text;
        [JsonIgnore]
        public override TierListTestItemContentType MatchingEnumType => TierListTestItemContentType.Text;

        public override IEnumerable<object> GetEqualityComponents() {
            yield return Text;
        }

        public override Dictionary<string, string> ToDictionary() => new() {
            ["Text"] = Text
        };

        public static ErrOr<TextOnly> CreateFromDictionary(Dictionary<string, string> dictionary) {
            if (!dictionary.TryGetValue("Text", out string text)) {
                return Err.ErrFactory.InvalidData("Unable to create item content. Text not provided");
            }

            if (!TierListTestItemContentTypeSpecificDataRules.IsStringCorrectItemText(text, out int textLength)) {
                return Err.ErrFactory.InvalidData(
                    $"Answer text must be between {TierListTestItemContentTypeSpecificDataRules.ItemTextMinLength} and {TierListTestItemContentTypeSpecificDataRules.ItemTextMaxLength} characters",
                    details: $"Current length: {textLength}"
                );
            }

            return new TextOnly(text);
        }
    }
}
