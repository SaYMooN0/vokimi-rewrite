using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.TestFeedbackRecordAggregate.general_test;

public class GeneralTestFeedbackRecord : BaseTestFeedbackRecord
{
    private GeneralTestFeedbackRecord() { }
    public bool WasLeftAnonymously { get; private init; }
    public string Text { get; private init; }

    public static ErrOr<GeneralTestFeedbackRecord> CreateNew(
        TestFeedbackRecordId FeedbackRecordId,
        TestId testId,
        AppUserId? userId,
        TestTakenRecordId testTakenRecordId,
        DateTime createdOn,
        string text,
        bool wasLeftAnonymously
    ) {
        if (!wasLeftAnonymously && userId is null) {
            return new Err(
                "Non-anonymous feedback cannot be created with user id unset",
                details: "User id is null"
            );
        }

        int textLen = string.IsNullOrEmpty(text) ? 0 : text.Length;
        if (textLen == 0) {
            return Err.ErrFactory.InvalidData(
                $"Feedback text cannot be empty"
            );
        }

        if (textLen > GeneralTestFeedbackRules.MaxPossibleFeedbackLength) {
            return Err.ErrFactory.InvalidData(
                $"Maximum feedback text length is {GeneralTestFeedbackRules.MaxPossibleFeedbackLength}. Current length is {textLen}"
            );
        }

        return new GeneralTestFeedbackRecord() {
            Id = FeedbackRecordId,
            TestId = testId,
            UserId = userId,
            TestTakenRecordId = testTakenRecordId,
            CreatedOn = createdOn,
            Text = text,
            WasLeftAnonymously = wasLeftAnonymously
        };
    }
}