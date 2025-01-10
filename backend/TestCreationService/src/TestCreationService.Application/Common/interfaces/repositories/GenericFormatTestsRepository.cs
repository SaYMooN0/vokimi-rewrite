
using SharedKernel.Common.EntityIds;

namespace TestCreationService.Application.Common.interfaces.repositories;

public interface IGenericFormatTestsRepository
{
    public Task CreateNew(AppUserId creatorId, string name, AppUserId[] editors = null);
}
