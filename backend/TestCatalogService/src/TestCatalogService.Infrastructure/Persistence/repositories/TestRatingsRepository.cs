using TestCatalogService.Application.Common.interfaces.repositories;

namespace TestCatalogService.Infrastructure.Persistence.repositories;

internal class TestRatingsRepository : ITestRatingsRepository
{
    private TestCatalogDbContext _db;
    public TestRatingsRepository(TestCatalogDbContext db) {
        _db = db;
    }
}