using System.Text.Json.Serialization;
using SharedKernel.Common.errors;

namespace SharedKernel.Common.tests.tier_list_format.items;

public abstract partial class TierListTestItemContentData
{
    public sealed class TextOnly : items.TierListTestItemContentData
    {
        public string Text { get; }

        private TextOnly(string text) => Text = text;
        [JsonIgnore] public override TierListTestItemContentType MatchingEnumType => TierListTestItemContentType.TextOnly;

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

            if (TierListTestItemRules.CheckIfStringCorrectItemTextContent(text).IsErr(out var err)) {
                return err;
            }

            return new TextOnly(text);
        }
    }
}