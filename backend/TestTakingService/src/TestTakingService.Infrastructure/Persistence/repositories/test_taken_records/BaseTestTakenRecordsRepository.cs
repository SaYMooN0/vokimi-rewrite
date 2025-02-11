using TestTakingService.Application.Common.interfaces.repositories.test_taken_records;
using TestTakingService.Domain.TestTakenRecordAggregate;

namespace TestTakingService.Infrastructure.Persistence.repositories.test_taken_records;

internal class BaseTestTakenRecordsRepository : IBaseTestTakenRecordsRepository
{
    private readonly TestTakingDbContext _db;

    public BaseTestTakenRecordsRepository(TestTakingDbContext db) {
        _db = db;
    }

    public async Task Add(BaseTestTakenRecord testTakenRecord) {
        await _db.BaseTestTakenRecords.AddAsync(testTakenRecord);
        await _db.SaveChangesAsync();
    }
}