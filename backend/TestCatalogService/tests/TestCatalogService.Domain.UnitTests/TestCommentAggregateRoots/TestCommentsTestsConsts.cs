using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Domain.UnitTests.TestCommentAggregateRoots;

public static class TestCommentsTestsConsts
{
    public static readonly TestId TestId = new(Guid.NewGuid());
    public static readonly AppUserId AuthorId = new(Guid.NewGuid());
    public static readonly string DefaultCommentText = "This is a default comment text.";
    public static readonly bool DefaultMarkAsSpoiler = false;
    public static IDateTimeProvider DateTimeProviderInstance = new DateTimeProvider();


    public static TestComment CreateNewComment(
        TestId? testId = null,
        AppUserId? authorId = null,
        string? text = null,
        TestCommentAttachment? attachment = null,
        bool? markAsSpoiler = null,
        IDateTimeProvider? dateTimeProvider = null
    ) {
        testId ??= TestId;
        authorId ??= AuthorId;
        text ??= DefaultCommentText;
        markAsSpoiler ??= DefaultMarkAsSpoiler;
        dateTimeProvider ??= DateTimeProviderInstance;

        return TestComment
            .CreateNew(testId, authorId, text, attachment, markAsSpoiler.Value, dateTimeProvider)
            .GetSuccess();
    }
}

file class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateOnly NowDateOnly => DateOnly.FromDateTime(DateTime.Now);
}