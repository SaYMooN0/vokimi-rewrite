using SharedKernel.Common.domain.entity;
using TestManagingService.Domain.AppUserAggregate;

namespace TestManagingService.Application.Common.interfaces.repositories;

public interface IAppUsersRepository
{
    public Task<AppUser?> GetById(AppUserId appUserId);
    public Task Add(AppUser appUser);
    public Task Update(AppUser appUser);
    
}