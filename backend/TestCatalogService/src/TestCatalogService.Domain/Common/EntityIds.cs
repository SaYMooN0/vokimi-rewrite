using SharedKernel.Common;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.TestTagAggregate;

namespace TestCatalogService.Domain.Common;

public class TestTagId : IEntityId
{
    public string Value { get; init; }

    public TestTagId(string value) {
        if (!TestTagsRules.IsStringValidTag(value)) {
            throw new ErrCausedException(Err.ErrFactory.InvalidData($"'{value}' is not a valid tag"));
        }

        Value = value;
    }

    public static ErrOr<TestTagId> Create(string value) {
        if (!TestTagsRules.IsStringValidTag(value)) {
            return Err.ErrFactory.InvalidData($"'{value}' is not a valid tag");
        }

        return new TestTagId(value);
    }

    public override string ToString() => Value;
}

public class TestRatingId : EntityId
{
    public TestRatingId(Guid value) : base(value) { }

    public static TestRatingId CreateNew() => new(Guid.CreateVersion7());
}

public class TestCommentId : EntityId
{
    public TestCommentId(Guid value) : base(value) { }

    public static TestCommentId CreateNew() => new(Guid.CreateVersion7());
}
public class CommentVoteId : EntityId
{
    public CommentVoteId(Guid value) : base(value) { }

    public static CommentVoteId CreateNew() => new(Guid.CreateVersion7());
}