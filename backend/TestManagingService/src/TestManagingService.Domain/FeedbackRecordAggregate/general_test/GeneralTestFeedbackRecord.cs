using SharedKernel.Common.domain.entity;
using SharedKernel.Common.interfaces;
using TestManagingService.Domain.Common;

namespace TestManagingService.Domain.FeedbackRecordAggregate.general_test;

public class GeneralTestFeedbackRecord : BaseTestFeedbackRecord
{
    private GeneralTestFeedbackRecord() { }
    public bool WasLeftAnonymously { get; private init; }
    public string Text { get; private init; }

    public static GeneralTestFeedbackRecord CreateNewAnonymous(
        TestId testId, DateTime createdOn, string text
    ) => new() {
        Id = TestFeedbackRecordId.CreateNew(),
        TestId = testId,
        UserId = null,
        CreatedOn = createdOn,
        Text = text,
        WasLeftAnonymously = true
    };

    public static GeneralTestFeedbackRecord CreateNewNonAnonymous(
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