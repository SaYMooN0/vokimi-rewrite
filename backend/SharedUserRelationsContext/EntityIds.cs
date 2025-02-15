using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;

namespace SharedUserRelationsContext;

public class UserFollowingId : EntityId
{
    public UserFollowingId(Guid value) : base(value) { }

    public static UserFollowingId CreateNew() => new(Guid.CreateVersion7());
}