using SharedKernel.Common.domain;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity;

namespace SharedUserRelationsContext.Entities;

public class UserRelationsAppUser : AggregateRoot<AppUserId>
{
    private UserRelationsAppUser() { }
    public List<UserFollowing> Followings { get; init; }
    public List<UserFollowing> Followers { get; init; }
}