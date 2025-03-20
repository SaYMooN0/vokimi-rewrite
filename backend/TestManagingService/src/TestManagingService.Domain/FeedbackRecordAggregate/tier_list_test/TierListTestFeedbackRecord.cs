using SharedKernel.Common.domain.entity;
using SharedKernel.Common.interfaces;
using TestManagingService.Domain.Common;

namespace TestManagingService.Domain.FeedbackRecordAggregate.tier_list_test;

public class TierListTestFeedbackRecord : BaseTestFeedbackRecord
{
    private TierListTestFeedbackRecord() { }
    public bool WasLeftAnonymously { get; private init; }
    public string Text { get; private init; }

    public static TierListTestFeedbackRecord CreateNewAnonymous(
        TestId testId, DateTime createdOn, string text
    ) => new() {
        Id = TestFeedbackRecordId.CreateNew(),
        TestId = testId,
        UserId = null,
        CreatedOn = createdOn,
        Text = text,
        WasLeftAnonymously = true
    };

    public static TierListTestFeedbackRecord CreateNewNonAnonymous(
        TestId testId, AppUserId userId, DateTime createdOn, string text
    ) => new() {
        Id = TestFeedbackRecordId.CreateNew(),
        TestId = testId,
        UserId = userId,
        CreatedOn = createdOn,
        Text = text,
        WasLeftAnonymously = false
    };
}