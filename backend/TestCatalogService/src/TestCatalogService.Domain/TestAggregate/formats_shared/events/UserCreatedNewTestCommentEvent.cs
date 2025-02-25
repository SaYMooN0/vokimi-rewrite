using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Domain.TestAggregate.formats_shared.events;

public record UserCreatedNewTestCommentEvent(TestCommentId CommentId, AppUserId AuthorId): IDomainEvent;