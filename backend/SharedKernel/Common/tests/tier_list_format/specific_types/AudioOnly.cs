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

        [JsonIgnore] public override TierListTestItemContentType MatchingEnumType => TierListTestItemContentType.AudioOnly;

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

            if (
                TierListTestItemRules
                .CheckIdStringCorrectRequiredNonTextItemContent(audio, "Audio")
                .IsErr(out var err)
            ) {
                return err;
            }

            dictionary.TryGetValue("Transcription", out string? transcription);

            if (
                TierListTestItemRules
                .CheckIfStringCorrectAudioTranscriptionItemContent(transcription).IsErr(out err)
            ) {
                return err;
            }

            return new AudioOnly(audio, transcription);
        }
    }
}