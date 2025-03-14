using System.Text.Json.Serialization;
using SharedKernel.Common.errors;

namespace SharedKernel.Common.general_test_questions.answer_type_specific_data;

public abstract partial class GeneralTestAnswerTypeSpecificData
{
    public sealed class TextOnly : GeneralTestAnswerTypeSpecificData
    {
        public string Text { get; }
        private TextOnly(string text) => Text = text;
        [JsonIgnore] public override GeneralTestAnswersType MatchingEnumType => GeneralTestAnswersType.TextOnly;

        public override IEnumerable<object> GetEqualityComponents() {
            yield return Text;
        }

        public override Dictionary<string, string> ToDictionary() =>
            new() { ["Text"] = Text };

        public static ErrOr<TextOnly> CreateFromDictionary(Dictionary<string, string> dictionary) {
            if (!dictionary.TryGetValue("Text", out string text)) {
                return Err.ErrFactory.InvalidData("Unable to create type specific data. Text not provided");
            }
            return CreateNew(text);
        }

        public static ErrOr<TextOnly> CreateNew(string text) {
            if (!GeneralTestAnswerTypeSpecificDataRules.IsStringCorrectAnswerText(text, out int textLength)) {
                return Err.ErrFactory.InvalidData(
                    $"Answer text must be between {GeneralTestAnswerTypeSpecificDataRules.AnswerMinLength} and {GeneralTestAnswerTypeSpecificDataRules.AnswerMaxLength} characters",
                    details: $"Current length: {textLength}"
                );
            }
            return new TextOnly(text);
        }
    }
}