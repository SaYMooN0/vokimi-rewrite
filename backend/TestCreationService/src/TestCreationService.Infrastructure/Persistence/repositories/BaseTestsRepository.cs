using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;

namespace TestCreationService.Infrastructure.Persistence.repositories;

internal class BaseTestsRepository : IBaseTestsRepository
{
    public Task<ErrOr<HashSet<AppUserId>>> CheckIfUserIsTestCreator(TestId testId, AppUserId userId) {
        throw new NotImplementedException();
    }

    public Task<ErrOr<HashSet<AppUserId>>> GetUserIdsWithPermissionToEditTest(TestId testId) {
        throw new NotImplementedException();
    }
}
