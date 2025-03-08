using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.Common.interfaces.repositories;
using TestCatalogService.Domain.TestTagAggregate;

namespace TestCatalogService.Infrastructure.Persistence.repositories;

internal class TestTagsRepository : ITestTagsRepository
{
    private TestCatalogDbContext _db;

    public TestTagsRepository(TestCatalogDbContext dbContext) {
        _db = dbContext;
    }

    public async Task AddNew(TestTag testTag) {
        _db.TestTags.Add(testTag);
        await _db.SaveChangesAsync();
    }

    public Task<string[]> TagIdValuesWithSubstring(string substring, int count) =>
        _db.TestTags
            .AsNoTracking()
            .Select(t => t.Id.Value)
            .Where(str => str.Contains(substring))
            .Take(count)
            .Order()
            .ToArrayAsync();

    public async Task<TestTag?> GetById(TestTagId id) {
        return await _db.TestTags.FindAsync(id);
    }

    public async Task Update(TestTag testTag) {
        _db.TestTags.Update(testTag);
        await _db.SaveChangesAsync();
    }
}