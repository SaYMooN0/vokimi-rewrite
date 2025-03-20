using SharedKernel.Common.domain.entity;
using TestManagingService.Application.Common.interfaces.repositories.feedback_records;
using TestManagingService.Domain.FeedbackRecordAggregate.tier_list_test;

namespace TestManagingService.Infrastructure.Persistence.repositories.feedback_records;

internal class TierListTestFeedbackRecordsRepository : ITierListTestFeedbackRecordsRepository
{
    private readonly TestManagingDbContext _db;

    public TierListTestFeedbackRecordsRepository(TestManagingDbContext db) {
        _db = db;
    }

    public async Task Add(TierListTestFeedbackRecord feedbackRecord) {
        await _db.TierListTestFeedbackRecords.AddAsync(feedbackRecord);
        await _db.SaveChangesAsync();
    }

    public Task<TierListTestFeedbackRecord[]> ListForTestAsNoTracking(TestId testId) => ;

    public Task<TierListTestFeedbackRecord[]> ListFilteredForTestAsNoTracking(
        TestId testId, TierListTestFeedbackRecordsFilter filter
        ) => ;
}