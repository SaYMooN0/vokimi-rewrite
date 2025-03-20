using SharedKernel.Common.domain.entity;
using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate.tier_list_format;

namespace TestManagingService.Infrastructure.Persistence.repositories.tests;

internal class TierListFormatTestsRepository : ITierListFormatTestsRepository
{
    private readonly TestManagingDbContext _db;

    public TierListFormatTestsRepository(TestManagingDbContext db) {
        _db = db;
    }

    public async Task<TierListFormatTest?> GetById(TestId testId) =>
        await _db.TierListFormatTests.FindAsync(testId);

    public async Task Add(TierListFormatTest tierListTest) {
        _db.TierListFormatTests.Add(tierListTest);
        await _db.SaveChangesAsync();
    }

    public async Task Update(TierListFormatTest test) {
        _db.TierListFormatTests.Update(test);
        await _db.SaveChangesAsync();
    }
}