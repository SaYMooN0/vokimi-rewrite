using System.Collections.Immutable;
using SharedKernel.Common.domain;

namespace SharedUserRelationsContext.repository;

public interface IUserFollowingsRepository
{
    public ImmutableArray<AppUserId> GetUserFollowings(AppUserId userId);
    public ImmutableArray<AppUserId> GetUserFollowers(AppUserId userId);
}