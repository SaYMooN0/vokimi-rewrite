using TestTakingService.Application.Common.interfaces.repositories.test_taken_records;
using TestTakingService.Domain.TestTakenRecordAggregate.tier_list_test;

namespace TestTakingService.Infrastructure.Persistence.repositories.test_taken_records;
 
internal class TierListTestTakenRecordsRepository : ITierListTestTakenRecordsRepository
{
    private readonly TestTakingDbContext _db;

    public TierListTestTakenRecordsRepository(TestTakingDbContext db) {
        _db = db;
    }

    public async Task Add(TierListTestTakenRecord tierListTestTakenRecord) {
        await _db.TierListTestTakenRecords.AddAsync(tierListTestTakenRecord);
        await _db.SaveChangesAsync();
    }
}