using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;

namespace TestCreationService.Application.Common.interfaces.repositories;

public interface IBaseTestsRepository
{
    //rethink this
    public Task<ErrOr<HashSet<AppUserId>>> GetUserIdsWithPermissionToEditTest(TestId testId);
    public Task<ErrOr<HashSet<AppUserId>>> CheckIfUserIsTestCreator(TestId testId, AppUserId userId);
}
