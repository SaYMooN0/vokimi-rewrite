using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;

namespace TestManagingService.Domain.TestAggregate.formats_shared.events;

public record class TestCommentReportedEvent(
    TestCommentReportId ReportId,
    AppUserId ReportAuthorId,
    TestCommentId CommentId
) : IDomainEvent;