using System.Text.Json.Serialization;
using SharedKernel.Common.errors;

namespace SharedKernel.Common.general_test_questions.answer_type_specific_data;

public abstract partial class GeneralTestAnswerTypeSpecificData
{
    public sealed class AudioAndText : GeneralTestAnswerTypeSpecificData
    {
        private AudioAndText() { }
        public string Audio { get; init; } = null!;
        public string Text { get; init; } = null!;
        [JsonIgnore]
        public override GeneralTestAnswersType MatchingEnumType => GeneralTestAnswersType.AudioAndText;

        public static ErrOr<AudioAndText> CreateNew(string text, string audio) {
            if (!GeneralTestAnswerTypeSpecificDataRules.IsStringCorrectAnswerText(text, out int textLength)) {
                return Err.ErrFactory.InvalidData(
                    $"Answer text must be between {GeneralTestAnswerTypeSpecificDataRules.AnswerMinLength} and {GeneralTestAnswerTypeSpecificDataRules.AnswerMaxLength} characters",
                    details: $"Current length: {textLength}"
                );
            }

            if (!GeneralTestAnswerTypeSpecificDataRules.IsStringCorrectNonTextItem(audio, out int audioLength)) {
                return Err.ErrFactory.InvalidData(
                    $"Audio data must be non-empty and at most {GeneralTestAnswerTypeSpecificDataRules.NonTextDataMaxLength} characters",
                    details: $"Current length: {audioLength}"
                );
            }

            return new AudioAndText { Text = text, Audio = audio };
        }

        public override IEnumerable<object> GetEqualityComponents() {
            yield return Audio;
            yield return Text;
        }
        public override Dictionary<string, string> ToDictionary() => new Dictionary<string, string> {
            ["Text"] = Text,
            ["Audio"] = Audio,
        };
        public static ErrOr<AudioAndText> CreateFromDictionary(Dictionary<string, string> dictionary) {
            if (!dictionary.TryGetValue("Text", out string text)) {
                return Err.ErrFactory.InvalidData("Unable to create type specific data. Text not provided");
            }
            if (!dictionary.TryGetValue("Audio", out string audio)) {
                return Err.ErrFactory.InvalidData("Unable to create type specific data. Audio not provided");
            }
            return CreateNew(text, audio);
        }
    }
}