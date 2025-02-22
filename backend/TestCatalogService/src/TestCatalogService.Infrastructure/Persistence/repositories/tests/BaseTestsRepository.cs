using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client.Events;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate;

namespace TestCatalogService.Infrastructure.Persistence.repositories.tests;

internal class BaseTestsRepository : IBaseTestsRepository
{
    private readonly TestCatalogDbContext _db;

    public BaseTestsRepository(TestCatalogDbContext db) {
        _db = db;
    }

    public async Task<BaseTest?> GetById(TestId testId) =>
        await _db.BaseTests.FindAsync(testId);

    public async Task<ErrOr<AppUserId>> GetTestCreatorId(TestId testId) {
        var test = await _db.BaseTests.FindAsync(testId);
        if (test is not null) {
            return test.CreatorId;
        }

        return Err.ErrPresets.TestNotFound(testId);
    }

    public async Task Update(BaseTest test) {
        _db.BaseTests.Update(test);
        await _db.SaveChangesAsync();
    }
}