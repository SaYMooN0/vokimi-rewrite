using TestCatalogService.Application.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate.general_format;

namespace TestCatalogService.Infrastructure.Persistence.repositories.tests;

internal class GeneralFormatTestsRepository : IGeneralFormatTestsRepository
{
    private readonly TestCatalogDbContext _db;

    public GeneralFormatTestsRepository(TestCatalogDbContext db) {
        _db = db;
    }

    public async Task Add(GeneralFormatTest generalTest) {
        _db.GeneralFormatTests.Add(generalTest);
        await _db.SaveChangesAsync();
    }
}