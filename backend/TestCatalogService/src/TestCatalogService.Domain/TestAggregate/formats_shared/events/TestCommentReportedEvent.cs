using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Domain.TestAggregate.formats_shared.events;

public record class TestCommentReportedEvent(
    TestCommentReportId ReportId,
    AppUserId ReportAuthorId,
    TestCommentId CommentId
) : IDomainEvent;