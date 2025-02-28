using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;

namespace TestManagingService.Domain.TestAggregate.formats_shared.comment_reports;

public class TestCommentReport : Entity<TestCommentReportId>
{
    private TestCommentReport() { }

    public AppUserId AuthorId { get; private init; }
    public TestCommentId CommentId { get; private init; }
    public CommentReportReason Reason { get; private init; }
    public string Text { get; init; }
    public DateTime CreatedAt { get; init; }

    public static ErrOr<TestCommentReport> CreateNew(
        AppUserId authorId,
        TestCommentId commentId,
        string text,
        CommentReportReason reason,
        IDateTimeProvider dateTimeProvider
    ) {
        if (TestCommentReportRules.CheckReportTextForErr(text).IsErr(out var textErr)) {
            return textErr;
        }

        return new TestCommentReport() {
            Id = TestCommentReportId.CreateNew(),
            AuthorId = authorId,
            CommentId = commentId,
            Reason = reason,
            Text = text,
            CreatedAt = dateTimeProvider.Now
        };
    }
}