using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate.general_format;

namespace TestManagingService.Infrastructure.Persistence.repositories.tests;

internal class GeneralFormatTestsRepository : IGeneralFormatTestsRepository
{
    private readonly TestManagingDbContext _db;

    public GeneralFormatTestsRepository(TestManagingDbContext db) {
        _db = db;
    }

    public async Task Add(GeneralFormatTest generalTest) {
        _db.GeneralFormatTests.Add(generalTest);
        await _db.SaveChangesAsync();
    }
}