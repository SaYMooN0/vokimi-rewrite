using TestTakingService.Application.Common.interfaces.repositories.test_taken_records;
using TestTakingService.Domain.TestTakenRecordAggregate.general_test;

namespace TestTakingService.Infrastructure.Persistence.repositories.test_taken_records;

internal class GeneralTestTakenRecordsRepository : IGeneralTestTakenRecordsRepository
{
    private readonly TestTakingDbContext _db;

    public GeneralTestTakenRecordsRepository(TestTakingDbContext db) {
        _db = db;
    }

    public async Task Add(GeneralTestTakenRecord generalTestTakenRecord) {
        await _db.GeneralTestTakenRecords.AddAsync(generalTestTakenRecord);
        await _db.SaveChangesAsync();
    }
}