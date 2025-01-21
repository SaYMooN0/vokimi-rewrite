namespace SharedKernel.Common.tests.value_objects.time_limit_option;

public abstract class BaseTimeLimitOption : ValueObject
{
    public bool TimeLimitExists { get; init; }
    public ushort? Seconds { get; init; }

    protected BaseTimeLimitOption(bool isEnabled, ushort? seconds) {
        TimeLimitExists = isEnabled;
        Seconds = seconds;
    }

    protected const int NoTimeLimitInt = -1;
    public int ToInt() => TimeLimitExists ? Seconds.Value : NoTimeLimitInt;
    public override IEnumerable<object> GetEqualityComponents()
        => TimeLimitExists ? [TimeLimitExists, Seconds] : [TimeLimitExists];
}