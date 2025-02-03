using TestCatalogService.Application.Common.interfaces.repositories;
using TestCatalogService.Domain.Common;
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

    public async Task<TestTag?> GetById(TestTagId id) {
        return await _db.TestTags.FindAsync(id);
    }

    public async Task Update(TestTag testTag) {
        _db.TestTags.Update(testTag);
        await _db.SaveChangesAsync();
    }
}
