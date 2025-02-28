using SharedKernel.Common.domain.entity;
using TestManagingService.Application.Common.interfaces.repositories.tests;

namespace TestManagingService.Infrastructure.Persistence.repositories.tests;

internal class BaseTestsRepository : IBaseTestsRepository
{
    private readonly TestManagingDbContext _db;

    public BaseTestsRepository(TestManagingDbContext db) {
        _db = db;
    }

    public async Task<BaseTest?> GetById(TestId testId) =>
        await _db.BaseTests.FindAsync(testId);

}