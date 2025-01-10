using SharedKernel.Common.EntityIds;

namespace TestCreationService.Application.Common.interfaces.repositories;

public interface IBaseTestsRepository
{
    public Task<bool> DoesUserHavePermissionToEdit(TestId testId, AppUserId appUserId);
}
