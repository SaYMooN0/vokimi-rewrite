using TestManagingService.Domain.TestFeedbackRecordAggregate.general_test;

namespace TestManagingService.Application.Common.interfaces.repositories.feedback_records;

public interface IGeneralTestFeedbackRecordsRepository
{
    public Task Add(GeneralTestFeedbackRecord feedbackRecord);
}