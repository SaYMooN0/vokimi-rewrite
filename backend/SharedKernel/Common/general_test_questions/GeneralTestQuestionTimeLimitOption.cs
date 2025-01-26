using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects;

namespace SharedKernel.Common.general_test_questions;
public sealed class GeneralTestQuestionTimeLimitOption : BaseTimeLimitOption
{
    private const ushort MinPossibleTimeLimitSeconds = 5;
    private const ushort MaxPossibleTimeLimitSeconds = 600;

    private GeneralTestQuestionTimeLimitOption(bool isEnabled, ushort? seconds) : base(isEnabled, seconds) { }

    public static GeneralTestQuestionTimeLimitOption NoTimeLimit()
        => new GeneralTestQuestionTimeLimitOption(false, null);

    public static ErrOr<GeneralTestQuestionTimeLimitOption> HasTimeLimit(ushort seconds) {
        if (seconds < MinPossibleTimeLimitSeconds) {
            return Err.ErrFactory.InvalidData(
                message: "Time limit too short.",
                details: $"Provided value '{seconds}' is below the minimum allowed time limit of {MinPossibleTimeLimitSeconds} seconds."
            );
        }

        if (seconds > MaxPossibleTimeLimitSeconds) {
            return Err.ErrFactory.InvalidData(
                message: "Time limit too long.",
                details: $"Provided value '{seconds}' exceeds the maximum allowed time limit of {MaxPossibleTimeLimitSeconds} seconds."
            );
        }

        return new GeneralTestQuestionTimeLimitOption(true, seconds);
    }

    public static new ErrOr<GeneralTestQuestionTimeLimitOption> FromInt(int value) {
        if (value == NoTimeLimitInt) {
            return NoTimeLimit();
        }

        if (value < MinPossibleTimeLimitSeconds) {
            return Err.ErrFactory.InvalidData(
                message: "Time limit too short.",
                details: $"Value '{value}' is below the minimum allowed time limit of {MinPossibleTimeLimitSeconds} seconds."
            );
        }

        if (value > MaxPossibleTimeLimitSeconds) {
            return Err.ErrFactory.InvalidData(
                message: "Time limit too long.",
                details: $"Value '{value}' exceeds the maximum allowed time limit of {MaxPossibleTimeLimitSeconds} seconds."
            );
        }

        return HasTimeLimit((ushort)value);
    }
}
