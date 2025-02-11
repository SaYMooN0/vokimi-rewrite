using TestTakingService.Application.Common.interfaces.repositories.feedback_records;
using TestTakingService.Domain.TestFeedbackRecordAggregate.general_test;

namespace TestTakingService.Infrastructure.Persistence.repositories.feedback_records;

internal class GeneralTestFeedbackRecordsRepository : IGeneralTestFeedbackRecordsRepository
{
    private readonly TestTakingDbContext _db;

    public GeneralTestFeedbackRecordsRepository(TestTakingDbContext db) {
        _db = db;
    }

    public async Task Add(GeneralTestFeedbackRecord feedbackRecord) {
        await _db.GeneralTestFeedbackRecords.AddAsync(feedbackRecord);
        await _db.SaveChangesAsync();
    }
}