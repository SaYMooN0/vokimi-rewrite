using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Infrastructure.Persistence.repositories;

internal class GeneralFormatTestsRepository : IGeneralFormatTestsRepository
{
    private readonly TestCreationDbContext _db;

    public GeneralFormatTestsRepository(TestCreationDbContext db) { _db = db; }
    public async Task AddNew(GeneralFormatTest test) {
        await _db.GeneralFormatTests.AddAsync(test);
        await _db.SaveChangesAsync();

    }
}
