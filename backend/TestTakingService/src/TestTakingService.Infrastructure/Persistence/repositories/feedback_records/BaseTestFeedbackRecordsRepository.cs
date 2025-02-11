using TestTakingService.Application.Common.interfaces.repositories.feedback_records;

namespace TestTakingService.Infrastructure.Persistence.repositories.feedback_records;

internal class BaseTestFeedbackRecordsRepository : IBaseTestFeedbackRecordsRepository
{
    private readonly TestTakingDbContext _db;
    public BaseTestFeedbackRecordsRepository(TestTakingDbContext db) {
        _db = db;
    }
}