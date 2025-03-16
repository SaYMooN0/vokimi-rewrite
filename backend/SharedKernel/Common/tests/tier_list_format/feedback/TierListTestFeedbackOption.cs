using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.value_object;
using SharedKernel.Common.errors;

namespace SharedKernel.Common.tests.tier_list_format.feedback;

public abstract class TierListTestFeedbackOption : ValueObject
{
    public sealed class Disabled : TierListTestFeedbackOption
    {
        public static Disabled Instance = new Disabled();
        private Disabled() { }

        public override IEnumerable<object> GetEqualityComponents() {
            yield return nameof(Disabled);
        }
    }

    public sealed class Enabled : TierListTestFeedbackOption
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
            if (
                TierListTestFeedbackRules.CheckAccompanyingTextForErrs(accompanyingText)
                    .IsErr(out var err)
                || TierListTestFeedbackRules.CheckMaxFeedbackLengthForErrs(maxFeedbackLength)
                    .IsErr(out err)
            ) {
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