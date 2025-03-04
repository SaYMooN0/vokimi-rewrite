using System.Collections;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestCreationService.Domain.AppUserAggregate;

namespace TestCreationService.Application.Common.interfaces.repositories;

public interface IAppUsersRepository
{
    public Task<AppUser?> GetById(AppUserId id);
    public Task Add(AppUser appUser);
    public Task Update(AppUser appUser);
    public Task UpdateRange(IEnumerable<AppUser> appUser);
}
