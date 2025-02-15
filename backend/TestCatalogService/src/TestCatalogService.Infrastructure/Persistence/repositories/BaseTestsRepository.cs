using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using TestCatalogService.Application.Common.interfaces.repositories;
using TestCatalogService.Application.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate;

namespace TestCatalogService.Infrastructure.Persistence.repositories;

internal class BaseTestsRepository : IBaseTestsRepository
{
    private readonly TestCatalogDbContext _db;

    public BaseTestsRepository(TestCatalogDbContext db) {
        _db = db;
    }

    public async Task<BaseTest?> GetById(TestId testId) =>
        await _db.BaseTests.FindAsync(testId);
}