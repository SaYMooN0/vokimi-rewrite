using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using SharedKernel.Common.tests.general_format;
using TestManagingService.Domain.Common;

namespace TestManagingService.Domain.TestFeedbackRecordAggregate.general_test;

public class GeneralTestFeedbackRecord : BaseTestFeedbackRecord
{
    private GeneralTestFeedbackRecord() { }
    public bool WasLeftAnonymously { get; private init; }
    public string Text { get; private init; }

    public static GeneralTestFeedbackRecord CreateNewAnonymous(
        TestId testId,
        IDateTimeProvider dateTimeProvider,
        string text
    ) => new() {
        Id = TestFeedbackRecordId.CreateNew(),
        TestId = testId,
        UserId = null,
        CreatedOn = dateTimeProvider.Now,
        Text = text,
        WasLeftAnonymously = true
    };

    public static GeneralTestFeedbackRecord CreateNewNonAnonymous(
        TestId testId,
        AppUserId userId,
        IDateTimeProvider dateTimeProvider,
        string text
    ) => new() {
        Id = TestFeedbackRecordId.CreateNew(),
        TestId = testId,
        UserId = userId,
        CreatedOn = dateTimeProvider.Now,
        Text = text,
        WasLeftAnonymously = false
    };
}