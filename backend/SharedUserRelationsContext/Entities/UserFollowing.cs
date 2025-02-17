using SharedKernel.Common.domain;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity;

namespace SharedUserRelationsContext.Entities;

public class UserFollowing : AggregateRoot<UserFollowingId>
{
    private UserFollowing() { }

    public AppUserId FollowerId { get; init; }
    public AppUserId FollowedUserId { get; init; }
    public DateTime FollowedAt { get; init; }
}