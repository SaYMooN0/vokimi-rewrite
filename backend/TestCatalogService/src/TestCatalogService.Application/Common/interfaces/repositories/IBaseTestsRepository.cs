using SharedKernel.Common.domain;
using TestCatalogService.Domain.TestAggregate;

namespace TestCatalogService.Application.Common.interfaces.repositories;

public interface IBaseTestsRepository
{
    public Task<BaseTest?> GetById(TestId testId);
}