using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.interfaces;
using TestManagingService.Domain.Common;

namespace TestManagingService.Domain.TestAggregate.formats_shared.comment_reports;

public class TestCommentReport : Entity<TestCommentReportId>
{
    private TestCommentReport() { }

    public AppUserId AuthorId { get; private init; }
    public TestCommentId CommentId { get; private init; }
    public CommentReportReason Reason { get; private init; }
    public string Text { get; init; }
    public DateTime CreatedAt { get; init; }

    public static TestCommentReport CreateNew(
        AppUserId authorId,
        TestCommentId commentId,
        string text,
        CommentReportReason reason,
        IDateTimeProvider dateTimeProvider
    ) {
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