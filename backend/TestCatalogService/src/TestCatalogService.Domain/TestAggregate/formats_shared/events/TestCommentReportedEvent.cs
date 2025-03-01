using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;

namespace TestCatalogService.Domain.TestAggregate.formats_shared.events;

public record class TestCommentReportedEvent(
    TestId TestId,
    AppUserId ReportAuthorId,
    TestCommentId CommentId,
    string Text,
    CommentReportReason Reason
) : IDomainEvent;