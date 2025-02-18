using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Domain.TestAggregate.formats_shared.events;

public record TestCommentToTestAddedEvent(TestCommentId CommentId, AppUserId AuthorId): IDomainEvent;