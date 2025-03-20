using SharedKernel.Common.domain.entity;
using TestTakingService.Application.Common.interfaces.repositories.tests;
using TestTakingService.Domain.TestAggregate.tier_list_format;

namespace TestTakingService.Infrastructure.Persistence.repositories.tests;

public class TierListFormatTestsRepository : ITierListFormatTestsRepository
{
    private readonly TestTakingDbContext _db;

    public TierListFormatTestsRepository(TestTakingDbContext db) {
        _db = db;
    }

    public async Task Add(TierListFormatTest test) {
        await _db.TierListFormatTests.AddAsync(test);
        await _db.SaveChangesAsync();
    }

    public async Task Update(TierListFormatTest test) {
        _db.TierListFormatTests.Update(test);
        await _db.SaveChangesAsync();
    }

    public async Task<TierListFormatTest?> GetById(TestId testId) =>
        await _db.TierListFormatTests.FindAsync(testId);
}