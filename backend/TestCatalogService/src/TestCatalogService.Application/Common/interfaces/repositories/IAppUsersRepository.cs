using SharedKernel.Common.domain;
using TestCatalogService.Domain.AppUserAggregate;

namespace TestCatalogService.Application.Common.interfaces.repositories;

public interface IAppUsersRepository
{
    public Task<AppUser?> GetById(AppUserId appUserId);
    public Task Add(AppUser appUser);
    public Task Update(AppUser appUser);
    
}