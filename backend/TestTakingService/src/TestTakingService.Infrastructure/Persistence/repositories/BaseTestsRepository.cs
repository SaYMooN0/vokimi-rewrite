using SharedKernel.Common.domain;
using TestTakingService.Application.Common.interfaces.repositories;
using TestTakingService.Domain.TestAggregate;

namespace TestTakingService.Infrastructure.Persistence.repositories;

public class BaseTestsRepository : IBaseTestsRepository
{
    private readonly TestTakingDbContext _db;

    public BaseTestsRepository(TestTakingDbContext db) {
        _db = db;
    }

    public async Task<BaseTest?> GetById(TestId testId) =>
        await _db.BaseTests.FindAsync(testId);
}