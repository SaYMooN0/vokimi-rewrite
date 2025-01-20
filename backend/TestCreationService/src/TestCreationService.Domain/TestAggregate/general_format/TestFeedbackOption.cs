using SharedKernel.Common;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Common.rules;

namespace TestCreationService.Domain.TestAggregate.general_format;

public abstract class TestFeedbackOption : ValueObject
{
    private TestFeedbackOption() { }
    public sealed class Disabled : TestFeedbackOption
    {
        private static Disabled _disabled;
        public static Disabled Instance => _disabled ??= new Disabled();
        private Disabled() { }

        public override IEnumerable<object> GetEqualityComponents() {
            yield return nameof(Disabled);
        }
    }

    public sealed class Enabled : TestFeedbackOption
    {
        public AnonymityValues Anonymity { get; }
        public string AccompanyingText { get; }
        public ushort MaxFeedbackLength { get; }

        private Enabled(AnonymityValues anonymity, string accompanyingText, ushort maxFeedbackLength) {
            Anonymity = anonymity;
            AccompanyingText = accompanyingText;
            MaxFeedbackLength = maxFeedbackLength;
        }

        public static ErrOr<Enabled> CreateNew(
            AnonymityValues anonymity,
            string accompanyingText,
            ushort maxFeedbackLength
        ) {
            if (IsStringCorrectAccompanyingText(accompanyingText).IsErr(out var err)) {
                return err;
            }
            if (IsValueCorrectMaxFeedbackLength(maxFeedbackLength).IsErr(out err)) {
                return err;
            }
            return new Enabled(anonymity, accompanyingText, maxFeedbackLength);
        }
        private static ErrOrNothing IsStringCorrectAccompanyingText(string str) {
            int len = string.IsNullOrWhiteSpace(str) ? 0 : str.Length;
            if (len == 0) {
                return Err.ErrFactory.InvalidData(
                    message: "Accompanying text cannot be empty."
                );
            }
            if (len > TestFeedbackRules.MaxPossibleFeedbackLength) {
                return Err.ErrFactory.InvalidData(
                    $"Accompanying text is too long. Maximum length is {TestFeedbackRules.MaxPossibleFeedbackLength}",
                    details: $"Maximum length is {TestFeedbackRules.MaxPossibleFeedbackLength}. Current length is {len}"
                );
            }
            return ErrOrNothing.Nothing;
        }
        private static ErrOrNothing IsValueCorrectMaxFeedbackLength(ushort value) {
            if (value == 0) {
                return Err.ErrFactory.InvalidData("Maximum Feedback Length must be greater than 0");
            }
            if (value > TestFeedbackRules.MaxPossibleFeedbackLength) {
                return Err.ErrFactory.InvalidData(
                    $"Maximum Feedback Length cannot be greater than {TestFeedbackRules.MaxPossibleFeedbackLength}"
                );
            }
            return ErrOrNothing.Nothing;
        }
        public override IEnumerable<object> GetEqualityComponents() {
            yield return Anonymity;
            yield return AccompanyingText;
            yield return MaxFeedbackLength;
        }
    }
}
