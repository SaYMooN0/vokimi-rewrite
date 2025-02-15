using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using TestCatalogService.Domain.TestAggregate;

namespace TestCatalogService.Application.Common.interfaces.repositories.tests;

public interface IBaseTestsRepository
{
    public Task<BaseTest?> GetById(TestId testId);
}