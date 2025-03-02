using TestManagingService.Application.Common.interfaces.repositories.feedback_records;

namespace TestManagingService.Infrastructure.Persistence.repositories.feedback_records;

internal class BaseTestFeedbackRecordsRepository : IBaseTestFeedbackRecordsRepository
{
    private readonly TestManagingDbContext _db;

    public BaseTestFeedbackRecordsRepository(TestManagingDbContext db) {
        _db = db;
    }
}