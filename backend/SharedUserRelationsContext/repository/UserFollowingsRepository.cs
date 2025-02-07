using System.Collections.Immutable;
using SharedKernel.Common.domain;

namespace SharedUserRelationsContext.repository;

internal class UserFollowingsRepository : IUserFollowingsRepository
{
    private readonly UserRelationsDbContext _db;

    public UserFollowingsRepository(UserRelationsDbContext db) => _db = db;

    public ImmutableArray<AppUserId> GetUserFollowings(AppUserId userId) { }

    public ImmutableArray<AppUserId> GetUserFollowers(AppUserId userId) { }
}