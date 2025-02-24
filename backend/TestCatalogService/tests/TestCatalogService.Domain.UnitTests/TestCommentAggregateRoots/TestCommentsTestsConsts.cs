using SharedKernel.Common.domain.entity;
using SharedKernel.Common.interfaces;

namespace TestCatalogService.Domain.UnitTests.TestCommentAggregateRoots;

public static class TestCommentsTestsConsts
{
    public static readonly TestId TestId = new(Guid.NewGuid());
    public static readonly AppUserId AuthorId = new(Guid.NewGuid());
    public static readonly string DefaultCommentText = "This is a default comment text.";
    public static readonly bool DefaultMarkAsSpoiler = false;
    public static IDateTimeProvider DateTimeProviderInstance = new DateTimeProvider();
}

file class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateOnly NowDateOnly => DateOnly.FromDateTime(DateTime.Now);
}