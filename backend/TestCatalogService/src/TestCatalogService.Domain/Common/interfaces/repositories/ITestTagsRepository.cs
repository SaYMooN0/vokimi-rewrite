using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.TestTagAggregate;

namespace TestCatalogService.Application.Common.interfaces.repositories;

public interface ITestTagsRepository
{
    public Task<TestTag?> GetById(TestTagId id);
    public Task Update(TestTag testTag);
    public Task AddNew(TestTag testTag);
}
