namespace SharedKernel.Common.interfaces;

public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateOnly NowDateOnly { get; }
}