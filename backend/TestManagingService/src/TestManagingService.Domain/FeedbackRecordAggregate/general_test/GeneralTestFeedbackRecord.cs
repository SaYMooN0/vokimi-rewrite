using SharedKernel.Common.domain.entity;
using TestManagingService.Domain.Common;

namespace TestManagingService.Domain.FeedbackRecordAggregate.general_test;

public class GeneralTestFeedbackRecord : BaseTestFeedbackRecord
{
    private GeneralTestFeedbackRecord() { }
    public string Text { get; }

    private GeneralTestFeedbackRecord(TestId testId, AppUserId? userId, DateTime createdOn, string text)
        : base(TestFeedbackRecordId.CreateNew(), testId, userId, createdOn) {
        Text = text;
    }

    public static GeneralTestFeedbackRecord CreateNewAnonymous(
        TestId testId, DateTime createdOn, string text
    ) => new(testId, null, createdOn, text);

    public static GeneralTestFeedbackRecord CreateNewNonAnonymous(
        TestId testId, AppUserId userId, DateTime createdOn, string text
    ) => new(testId, userId, createdOn, text);
}