using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.domain;
using SharedUserRelationsContext.Entities;

namespace SharedUserRelationsContext.repository;

internal class UserFollowingsRepository : IUserFollowingsRepository
{
    private readonly UserRelationsDbContext _db;

    public UserFollowingsRepository(UserRelationsDbContext db) => _db = db;

    public async Task<ImmutableArray<AppUserId>> GetUserFollowings(AppUserId userId) =>
        (await _db.UserFollowings
            .AsNoTracking()
            .Where(f => f.FollowerId == userId)
            .Select(f => f.FollowedUserId)
            .ToArrayAsync())
        .ToImmutableArray();

    public async Task<ImmutableArray<AppUserId>> GetUserFollowers(AppUserId userId) =>
        (await _db.UserFollowings
            .AsNoTracking()
            .Where(f => f.FollowedUserId == userId)
            .Select(f => f.FollowerId)
            .ToArrayAsync())
        .ToImmutableArray();
}