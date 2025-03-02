using TestManagingService.Application.Common.interfaces.repositories.feedback_records;
using TestManagingService.Domain.TestFeedbackRecordAggregate.general_test;

namespace TestManagingService.Infrastructure.Persistence.repositories.feedback_records;

public class GeneralTestFeedbackRecordsRepository: IGeneralTestFeedbackRecordsRepository
{
    private readonly TestManagingDbContext _db;

    public GeneralTestFeedbackRecordsRepository(TestManagingDbContext db) {
        _db = db;
    }

    public async Task Add(GeneralTestFeedbackRecord feedbackRecord) {
        await _db.GeneralTestFeedbackRecords.AddAsync(feedbackRecord);
        await _db.SaveChangesAsync();
    }
}