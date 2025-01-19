using SharedKernel.Common.errors;

namespace SharedKernel.Common.tests.value_objects;

public class TimeLimitOption : ValueObject
{
    private const int NoTimeLimitInt = -1;
    public bool TimeLimitExists { get; init; }
    public ushort? Seconds { get; init; }

    public TimeLimitOption(bool isEnabled, ushort? seconds) {
        TimeLimitExists = isEnabled;
        Seconds = seconds;
    }

    public static TimeLimitOption NoTimeLimit() => new(false, null);

    public static TimeLimitOption HasTimeLimit(ushort maxSeconds) => new(true, maxSeconds);

    public override IEnumerable<object> GetEqualityComponents()
        => TimeLimitExists ? [TimeLimitExists, Seconds] : [TimeLimitExists];

    public static ErrOr<TimeLimitOption> FromInt(int value) {
        if (value == NoTimeLimitInt) { return NoTimeLimit(); }

        if (value >= 0 && value <= ushort.MaxValue) {
            return HasTimeLimit((ushort)value);
        }

        return Err.ErrFactory.InvalidData(
            message: "Invalid time limit value.",
            details: $"Value '{value}' is out of range for time limit"
        );
    }

    public int ToInt() => TimeLimitExists ? Seconds.Value : NoTimeLimitInt;
}