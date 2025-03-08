using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.TestTagAggregate;

namespace TestCatalogService.Domain.Common.interfaces.repositories;

public interface ITestTagsRepository
{
    public Task<TestTag?> GetById(TestTagId id);
    public Task Update(TestTag testTag);
    public Task AddNew(TestTag testTag);
    public Task<string[]> TagIdValuesWithSubstring(string substring, int count);
}