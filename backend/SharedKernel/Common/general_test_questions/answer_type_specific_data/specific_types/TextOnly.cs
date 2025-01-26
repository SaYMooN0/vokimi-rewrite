using SharedKernel.Common.errors;
using System.Text.Json.Serialization;

namespace SharedKernel.Common.general_test_questions.answer_type_specific_data;
public abstract partial class GeneralTestAnswerTypeSpecificData
{
    public sealed class TextOnly : GeneralTestAnswerTypeSpecificData
    {
        private TextOnly() { }
        public string Text { get; init; } = null!;
        [JsonIgnore]
        public override GeneralTestAnswersType MatchingEnumType => GeneralTestAnswersType.TextOnly;

        public static ErrOr<TextOnly> CreateNew(string text) {
            if (!GeneralTestAnswerTypeSpecificDataRules.IsStringCorrectAnswerText(text, out int textLength)) {
                return Err.ErrFactory.InvalidData(
                    $"Answer text must be between {GeneralTestAnswerTypeSpecificDataRules.AnswerMinLength} and {GeneralTestAnswerTypeSpecificDataRules.AnswerMaxLength} characters",
                    details: $"Current length: {textLength}"
                );
            }

            return new TextOnly { Text = text };
        }

        public override IEnumerable<object> GetEqualityComponents() {
            yield return Text;
        }
        public override Dictionary<string, string> ToDictionary() => new Dictionary<string, string> {
            ["Text"] = Text,
        };

        public static ErrOr<TextOnly> CreateFromDictionary(Dictionary<string, string> dictionary) {
            if (!dictionary.TryGetValue("Text", out string text)) {
                return Err.ErrFactory.InvalidData("Unable to create type specific data. Text not provided");
            }
            return CreateNew(text);
        }

    }
}