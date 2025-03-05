using SharedKernel.Common.domain.entity;
using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate;

namespace TestManagingService.Infrastructure.Persistence.repositories.tests;

internal class BaseTestsRepository : IBaseTestsRepository
{
    private readonly TestManagingDbContext _db;

    public BaseTestsRepository(TestManagingDbContext db) {
        _db = db;
    }

    public async Task<BaseTest?> GetById(TestId testId) =>
        await _db.BaseTests.FindAsync(testId);

    public async Task Update(BaseTest test) {
        _db.BaseTests.Update(test);
        await _db.SaveChangesAsync();
    }
}