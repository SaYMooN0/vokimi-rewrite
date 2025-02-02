using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;

namespace SharedKernel.Common.tests.general_format;

public abstract class GeneralTestFeedbackOption : ValueObject
{
    private GeneralTestFeedbackOption() { }
    public sealed class Disabled : GeneralTestFeedbackOption
    {
        private static Disabled _disabled;
        public static Disabled Instance => _disabled ??= new Disabled();
        private Disabled() { }
        public override IEnumerable<object> GetEqualityComponents() {
            yield return nameof(Disabled);
        }
    }

    public sealed class Enabled : GeneralTestFeedbackOption
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
            if (GeneralTestFeedbackRules.CheckAccompanyingTextForErrs(accompanyingText).IsErr(out var err)) {
                return err;
            }
            if (GeneralTestFeedbackRules.CheckMaxFeedbackLengthForErrs(maxFeedbackLength).IsErr(out err)) {
                return err;
            }
            return new Enabled(anonymity, accompanyingText, maxFeedbackLength);
        }

        public override IEnumerable<object> GetEqualityComponents() {
            yield return Anonymity;
            yield return AccompanyingText;
            yield return MaxFeedbackLength;
        }
    }
}
