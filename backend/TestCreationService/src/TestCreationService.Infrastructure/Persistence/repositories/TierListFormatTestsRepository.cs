using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.domain.entity;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Infrastructure.Persistence.repositories;

public class TierListFormatTestsRepository : ITierListFormatTestsRepository
{
    private readonly TestCreationDbContext _db;

    public TierListFormatTestsRepository(TestCreationDbContext db) {
        _db = db;
    }

    public async Task AddNew(TierListFormatTest test) {
        await _db.TierListFormatTests.AddAsync(test);
        await _db.SaveChangesAsync();
    }

    public async Task<TierListFormatTest?> GetById(TestId testId) =>
        await _db.TierListFormatTests.FindAsync(testId);

    public async Task Update(TierListFormatTest test) {
        _db.TierListFormatTests.Update(test);
        await _db.SaveChangesAsync();
    }

    public async Task<TierListFormatTest?> GetWithItemsIncluded(TestId testId) =>
        await _db.TierListFormatTests
            .Include(t =>
                EF.Property<ICollection<TierListTestItem>>(t, "_items")
            )
            .FirstOrDefaultAsync(t => t.Id == testId);
    
    public async Task<TierListFormatTest?> GetWithTiersIncluded(TestId testId) =>
        await _db.TierListFormatTests
            .Include(t =>
                EF.Property<ICollection<TierListTestTier>>(t, "_tiers")
            )
            .FirstOrDefaultAsync(t => t.Id == testId);
}