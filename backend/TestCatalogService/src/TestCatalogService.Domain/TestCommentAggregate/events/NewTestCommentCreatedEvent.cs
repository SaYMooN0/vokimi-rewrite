using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Domain.TestCommentAggregate.events;

public record NewTestCommentCreatedEvent(
    TestCommentId CommentId,
    TestId TestId,
    AppUserId AuthorId
) : IDomainEvent;