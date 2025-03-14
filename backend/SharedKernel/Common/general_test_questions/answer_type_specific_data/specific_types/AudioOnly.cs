using System.Text.Json.Serialization;
using SharedKernel.Common.errors;

namespace SharedKernel.Common.general_test_questions.answer_type_specific_data;

public abstract partial class GeneralTestAnswerTypeSpecificData
{
    public sealed class AudioOnly : GeneralTestAnswerTypeSpecificData
    {
        public string Audio { get; }
        private AudioOnly(string audio) => Audio = audio;
        [JsonIgnore] public override GeneralTestAnswersType MatchingEnumType => GeneralTestAnswersType.AudioOnly;

        public override IEnumerable<object> GetEqualityComponents() {
            yield return Audio;
        }

        public override Dictionary<string, string> ToDictionary() =>
            new() { ["Audio"] = Audio };

        public static ErrOr<AudioOnly> CreateFromDictionary(Dictionary<string, string> dictionary) {
            if (!dictionary.TryGetValue("Audio", out string audio)) {
                return Err.ErrFactory.InvalidData("Unable to create type specific data. Audio not provided");
            }

            return CreateNew(audio);
        }

        public static ErrOr<AudioOnly> CreateNew(string audio) {
            if (
                !GeneralTestAnswerTypeSpecificDataRules
                    .IsStringCorrectNonTextItem(audio, out int audioLength)
            ) {
                return Err.ErrFactory.InvalidData(
                    $"Audio data must be non-empty and at most {GeneralTestAnswerTypeSpecificDataRules.NonTextDataMaxLength} characters",
                    details: $"Current length: {audioLength}"
                );
            }

            return new AudioOnly(audio);
        }
    }
}