using SharedKernel.Common.domain.entity;
using TestManagingService.Domain.FeedbackRecordAggregate.general_test;

namespace TestManagingService.Application.Common.interfaces.repositories.feedback_records;

public interface IGeneralTestFeedbackRecordsRepository
{
    public Task Add(GeneralTestFeedbackRecord feedbackRecord);
    public Task<GeneralTestFeedbackRecord[]> ListForTestAsNoTracking(TestId testId);

    public Task<GeneralTestFeedbackRecord[]> ListFilteredForTestAsNoTracking(
        TestId testId, GeneralTestFeedbackRecordsFilter filter
    );
}