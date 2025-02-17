using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.TestAggregate;

namespace TestCatalogService.Application.Common.interfaces.repositories.tests;

public interface IBaseTestsRepository
{
    public Task<BaseTest?> GetById(TestId testId);
    public Task Update(BaseTest test);
}