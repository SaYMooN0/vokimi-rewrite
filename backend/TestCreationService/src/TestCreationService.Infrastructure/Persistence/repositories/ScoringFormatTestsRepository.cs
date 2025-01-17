using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.scoring_format;

namespace TestCreationService.Infrastructure.Persistence.repositories;

internal class ScoringFormatTestsRepository : IScoringFormatTestsRepository
{
    private readonly TestCreationDbContext _db;
    public ScoringFormatTestsRepository(TestCreationDbContext db) { _db = db; }

    public async Task AddNew(ScoringFormatTest test) {
        await _db.ScoringFormatTests.AddAsync(test);
        await _db.SaveChangesAsync();
    }
}
