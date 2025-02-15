using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.errors;
using TestCreationService.Domain.TestAggregate;

namespace TestCreationService.Application.Common.interfaces.repositories;

public interface IBaseTestsRepository
{
    public Task<ErrOr<HashSet<AppUserId>>> GetUserIdsWithPermissionToEditTest(TestId testId);
    public Task<ErrOr<bool>> CheckIfUserIsTestCreator(TestId testId, AppUserId userId);
    public Task<BaseTest?> GetById(TestId testId);
    public Task<BaseTest?> GetWithStyles(TestId testId);
    public Task<BaseTest?> GetWithTags(TestId testId);
    public Task Update(BaseTest test);
}
