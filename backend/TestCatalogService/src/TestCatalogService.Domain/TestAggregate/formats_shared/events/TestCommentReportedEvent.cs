using SharedKernel.Common.domain;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Domain.TestAggregate.formats_shared.events;

public record class TestCommentReportedEvent(TestCommentId CommentId) : IDomainEvent;