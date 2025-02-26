using SharedKernel.Common.interfaces;

namespace TestCatalogService.Domain.UnitTests;

public static class TestsSharedConsts
{
    public static IDateTimeProvider DateTimeProviderInstance = new DateTimeProvider();
}

file class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateOnly NowDateOnly => DateOnly.FromDateTime(DateTime.Now);
}