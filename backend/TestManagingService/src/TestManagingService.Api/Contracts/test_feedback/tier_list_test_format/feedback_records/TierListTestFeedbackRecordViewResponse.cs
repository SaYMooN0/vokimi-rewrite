using TestManagingService.Domain.FeedbackRecordAggregate.tier_list_test;

namespace TestManagingService.Api.Contracts.test_feedback.tier_list_test_format.feedback_records;

public record TierListTestFeedbackRecordViewResponse(
    string Text,
    bool IsAnonymous,
    string? AppUserId,
    DateTime CreatedOn
)
{
    public static TierListTestFeedbackRecordViewResponse FromFeedbackRecord(TierListTestFeedbackRecord record) =>
        new(record.Text, record.WasLeftAnonymously, record.UserId?.ToString() ?? null, record.CreatedOn);
}