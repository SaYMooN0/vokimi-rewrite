using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using TestTakingService.Domain.AppUserAggregate;

namespace TestTakingService.Application.Common.interfaces.repositories;

public interface IAppUsersRepository
{
    public Task<AppUser?> GetById(AppUserId appUserId);
    public Task Add(AppUser appUser);
    public Task Update(AppUser appUser);
}