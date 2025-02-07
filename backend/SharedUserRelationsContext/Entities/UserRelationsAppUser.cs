using SharedKernel.Common.domain;

namespace SharedUserRelationsContext.Entities;

public class UserRelationsAppUser : AggregateRoot<AppUserId>
{
    private UserRelationsAppUser() { }
    public List<UserFollowing> Followings { get; init; }
    public List<UserFollowing> Followers { get; init; }
}