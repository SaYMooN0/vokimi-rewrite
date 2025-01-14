using SharedKernel.Common.EntityIds;
using TestCreationService.Domain.AppUserAggregate;

namespace TestCreationService.Application.Common.interfaces.repositories;

public interface IAppUsersRepository
{
    public Task<AppUser?> GetById(AppUserId id);
}
