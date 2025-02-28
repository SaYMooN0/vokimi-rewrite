using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;

namespace TestManagingService.Domain.TestAggregate.formats_shared.events;

public record UserCreatedNewTestCommentEvent(TestCommentId CommentId, AppUserId AuthorId): IDomainEvent;