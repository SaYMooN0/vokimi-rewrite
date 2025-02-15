using System.Collections.Immutable;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;

namespace SharedUserRelationsContext.repository;

public interface IUserFollowingsRepository
{
    public Task<ImmutableArray<AppUserId>> GetUserFollowings(AppUserId userId);
    public Task<ImmutableArray<AppUserId>> GetUserFollowers(AppUserId userId);
}