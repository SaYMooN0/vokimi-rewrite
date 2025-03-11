using System.Collections.Immutable;
using SharedKernel.Common.domain.entity;
using SharedUserRelationsContext.Entities;
using SharedUserRelationsContext.repository;

namespace TestCatalogService.Domain.UnitTests.FakeRepositories;

public class FakeUserFollowingsRepository : IUserFollowingsRepository
{
    private readonly List<UserFollowing> _followings;

    public FakeUserFollowingsRepository(List<UserFollowing>? followings = null) {
        _followings = followings ?? [];
    }

    public Task<ImmutableArray<AppUserId>> GetUserFollowings(AppUserId userId) =>
        Task.FromResult(_followings
            .Where(f => f.FollowerId == userId)
            .Select(f => f.FollowedUserId)
            .ToImmutableArray());

    public Task<ImmutableArray<AppUserId>> GetUserFollowers(AppUserId userId) =>
        Task.FromResult(_followings
            .Where(f => f.FollowedUserId == userId)
            .Select(f => f.FollowerId)
            .ToImmutableArray());

    public Task AddFollowing(AppUserId followerId, AppUserId followedUserId) {
        _followings.Add(new UserFollowing(
            followerId, followedUserId, DateTime.UtcNow
        ));

        return Task.CompletedTask;
    }
}