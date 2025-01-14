using SharedKernel.Common.EntityIds;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.AppUserAggregate;

namespace TestCreationService.Infrastructure.Persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    public Task<AppUser?> GetById(AppUserId id) {
        throw new NotImplementedException();
    }
}
