using SharedKernel.Common.domain.entity;
using TestManagingService.Domain.FeedbackRecordAggregate.tier_list_test;

namespace TestManagingService.Application.Common.interfaces.repositories.feedback_records;

public interface ITierListTestFeedbackRecordsRepository
{
    public Task Add(TierListTestFeedbackRecord feedbackRecord);
    public Task<TierListTestFeedbackRecord[]> ListForTestAsNoTracking(TestId testId);

    public Task<TierListTestFeedbackRecord[]> ListFilteredForTestAsNoTracking(
        TestId testId, TierListTestFeedbackRecordsFilter filter
    );
}