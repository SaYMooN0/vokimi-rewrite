using AuthenticationService.Infrastructure.Services;
using SharedKernel.Common.interfaces;

namespace DBSeeder.Data;

internal static class DataShared
{
    public static readonly PasswordHasher PasswordHasher = new();
    public static IDateTimeProvider DateTimeProviderWithNowSet(DateTime now) =>
        new DateTimeProviderWithNowSet { Now = now };
}

file class DateTimeProviderWithNowSet : IDateTimeProvider
{
    public DateTime Now { get; init; }
    public DateOnly NowDateOnly => DateOnly.FromDateTime(Now);
}