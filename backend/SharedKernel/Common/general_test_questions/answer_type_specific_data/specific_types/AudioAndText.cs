using System.Text.Json.Serialization;
using SharedKernel.Common.errors;

namespace SharedKernel.Common.general_test_questions.answer_type_specific_data;

public abstract partial class GeneralTestAnswerTypeSpecificData
{
    public sealed class AudioAndText : GeneralTestAnswerTypeSpecificData
    {
        public string Audio { get; }
        public string Text { get; }
        private AudioAndText(string audio, string text) {
            Audio = audio;
            Text = text;
        }

        [JsonIgnore] public override GeneralTestAnswersType MatchingEnumType => GeneralTestAnswersType.AudioAndText;

        public override IEnumerable<object> GetEqualityComponents() {
            yield return Audio;
            yield return Text;
        }

        public override Dictionary<string, string> ToDictionary() => new() {
            ["Text"] = Text,
            ["Audio"] = Audio,
        };

        public static ErrOr<AudioAndText> CreateFromDictionary(Dictionary<string, string> dictionary) {
            if (!dictionary.TryGetValue("Text", out string text)) {
                return Err.ErrFactory.InvalidData("Unable to create type specific data. Text not provided");
            }

            if (!GeneralTestAnswerTypeSpecificDataRules.IsStringCorrectAnswerText(text, out int textLength)) {
                return Err.ErrFactory.InvalidData(
                    $"Answer text must be between {GeneralTestAnswerTypeSpecificDataRules.AnswerMinLength} and {GeneralTestAnswerTypeSpecificDataRules.AnswerMaxLength} characters",
                    details: $"Current length: {textLength}"
                );
            }

            if (!dictionary.TryGetValue("Audio", out string audio)) {
                return Err.ErrFactory.InvalidData("Unable to create type specific data. Audio not provided");
            }

            if (!GeneralTestAnswerTypeSpecificDataRules.IsStringCorrectNonTextItem(audio, out int audioLength)) {
                return Err.ErrFactory.InvalidData(
                    $"Audio data must be non-empty and at most {GeneralTestAnswerTypeSpecificDataRules.NonTextDataMaxLength} characters",
                    details: $"Current length: {audioLength}"
                );
            }

            return new AudioAndText(audio, text);
        }
    }
}