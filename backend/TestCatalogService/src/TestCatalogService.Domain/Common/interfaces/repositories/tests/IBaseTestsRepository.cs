using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.TestAggregate;

namespace TestCatalogService.Domain.Common.interfaces.repositories.tests;

public interface IBaseTestsRepository
{
    public Task<BaseTest?> GetById(TestId testId);
    public Task<ErrOr<AppUserId>> GetTestCreatorId(TestId testId);
    public Task Update(BaseTest test);
}