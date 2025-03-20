using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate.tier_list_format;

namespace TestCatalogService.Infrastructure.Persistence.repositories.tests;

public class TierListFormatTestsRepository : ITierListFormatTestsRepository
{
    private readonly TestCatalogDbContext _db;

    public TierListFormatTestsRepository(TestCatalogDbContext db) {
        _db = db;
    }

    public async Task Add(TierListFormatTest tierListTest) {
        _db.TierListFormatTests.Add(tierListTest);
        await _db.SaveChangesAsync();
    }
}