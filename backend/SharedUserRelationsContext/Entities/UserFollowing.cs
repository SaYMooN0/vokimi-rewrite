using SharedKernel.Common.domain;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity;

namespace SharedUserRelationsContext.Entities;

public class UserFollowing : AggregateRoot<UserFollowingId>
{
    private UserFollowing() { }
    public AppUserId FollowerId { get; }
    public AppUserId FollowedUserId { get; }
    public DateTime FollowedAt { get; }

    public UserFollowing(AppUserId followerId, AppUserId followedUserId, DateTime followedAt) {
        Id = UserFollowingId.CreateNew();
        FollowerId = followerId;
        FollowedUserId = followedUserId;
        FollowedAt = followedAt;
    }
}