using System.Text.Json.Serialization;
using SharedKernel.Common.errors;

namespace SharedKernel.Common.tests.tier_list_format;

public abstract partial class TierListTestItemContentData
{
    public sealed class AudioOnly : TierListTestItemContentData
    {
        public string Audio { get; }
        public string? Transcription { get; }
        private AudioOnly(string audio, string? transcription) {
            Audio = audio;
            Transcription = transcription;
        }

        [JsonIgnore] public override TierListTestItemContentType MatchingEnumType => TierListTestItemContentType.Audio;

        public override IEnumerable<object> GetEqualityComponents() {
            yield return Audio;
            yield return Transcription;
        }

        public override Dictionary<string, string> ToDictionary() => new() {
            ["Audio"] = Audio,
            ["Transcription"] = Transcription ?? string.Empty,
        };

        public static ErrOr<AudioOnly> CreateFromDictionary(Dictionary<string, string> dictionary) {
            if (!dictionary.TryGetValue("Audio", out string audio)) {
                return Err.ErrFactory.InvalidData("Unable to create type specific data. Audio not provided");
            }

            if (!TierListTestItemContentTypeSpecificDataRules.IsStringCorrectNonTextItem(audio, out int audioLen)) {
                return Err.ErrFactory.InvalidData(
                    $"Audio data must be non-empty and at most {TierListTestItemContentTypeSpecificDataRules.NonTextDataMaxLength} characters",
                    details: $"Current length: {audioLen}"
                );
            }

            dictionary.TryGetValue("Transcription", out string? transcription);


            if (!TierListTestItemContentTypeSpecificDataRules
                    .IsStringCorrectAudioTranscriptionItemContent(transcription, out int transcriptionLen)
               ) {
                return Err.ErrFactory.InvalidData(
                    $"Audio transcription data must be shorter than {TierListTestItemContentTypeSpecificDataRules.NonTextDataMaxLength} characters",
                    details: $"Current length: {transcriptionLen}"
                );
            }

            return new AudioOnly(audio, transcription);
        }
    }
}