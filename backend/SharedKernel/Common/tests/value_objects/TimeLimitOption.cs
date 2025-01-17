namespace SharedKernel.Common.tests.value_objects;

public class TimeLimitOption : ValueObject
{
    public bool TimeLimitExists { get; init; }
    public ushort? MaxSeconds { get; init; }

    public TimeLimitOption(bool isEnabled, ushort? maxSeconds) {
        TimeLimitExists = isEnabled;
        MaxSeconds = maxSeconds;
    }

    public static TimeLimitOption NoTimeLimit() => new(false, null);

    public static TimeLimitOption HasTimeLimit(ushort maxSeconds) => new(true, maxSeconds);

    public override IEnumerable<object> GetEqualityComponents()
        => TimeLimitExists ? [TimeLimitExists, MaxSeconds] : [TimeLimitExists];
}