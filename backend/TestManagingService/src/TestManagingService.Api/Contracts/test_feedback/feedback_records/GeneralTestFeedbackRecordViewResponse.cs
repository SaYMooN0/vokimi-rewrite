using TestManagingService.Domain.TestFeedbackRecordAggregate.general_test;

namespace TestManagingService.Api.Contracts.test_feedback.feedback_records;

public record GeneralTestFeedbackRecordViewResponse(
    string Text,
    bool IsAnonymous,
    string? AppUserId,
    DateTime CreatedOn
)
{
    public static GeneralTestFeedbackRecordViewResponse FromFeedbackRecord(GeneralTestFeedbackRecord record) =>
        new(record.Text, record.WasLeftAnonymously, record.UserId?.ToString() ?? null, record.CreatedOn);
}