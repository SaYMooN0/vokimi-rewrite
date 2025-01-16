using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Domain.TestAggregate;

namespace TestCreationService.Application.Common.interfaces.repositories;

public interface IBaseTestsRepository
{
    //rethink this
    public Task<ErrOr<HashSet<AppUserId>>> GetUserIdsWithPermissionToEditTest(TestId testId);
    public Task<ErrOr<bool>> CheckIfUserIsTestCreator(TestId testId, AppUserId userId);
    public Task<BaseTest?> GetById(TestId testId);
    public Task Update(BaseTest test);
}
