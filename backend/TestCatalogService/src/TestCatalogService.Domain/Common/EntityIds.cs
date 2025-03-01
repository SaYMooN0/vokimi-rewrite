using SharedKernel.Common.domain.entity;

namespace TestCatalogService.Domain.Common;


public class TestRatingId : EntityId
{
    public TestRatingId(Guid value) : base(value) { }

    public static TestRatingId CreateNew() => new(Guid.CreateVersion7());
}


public class CommentVoteId : EntityId
{
    public CommentVoteId(Guid value) : base(value) { }

    public static CommentVoteId CreateNew() => new(Guid.CreateVersion7());
}