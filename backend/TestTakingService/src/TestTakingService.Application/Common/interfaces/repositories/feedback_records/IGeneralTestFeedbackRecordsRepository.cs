using TestTakingService.Domain.TestFeedbackRecordAggregate.general_test;

namespace TestTakingService.Application.Common.interfaces.repositories.feedback_records;

public interface IGeneralTestFeedbackRecordsRepository
{
    public Task Add(GeneralTestFeedbackRecord feedbackRecord);
}