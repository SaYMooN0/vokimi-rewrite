using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using SharedKernel.Common.tests.tier_list_format.feedback;
using TestTakingService.Domain.Common.test_taken_data.tier_list_format_test;

namespace TestTakingService.Domain.TestAggregate.tier_list_format.helpers;

internal static class TierListTestTakingValidatorHelper
{
    internal static ErrOrNothing CheckForTestTimeDurationErrs(
        IDateTimeProvider dateTimeProvider,
        DateTime testStartTime,
        DateTime testEndTime
    ) {
        if (testStartTime > testEndTime) {
            return new Err("Test start time cannot be later than end time");
        }

        if (testStartTime > dateTimeProvider.Now) {
            return new Err("Test taking couldn't start in a future");
        }
        return ErrOrNothing.Nothing;
    }
    internal static ErrOrNothing ValidatePossibleFeedbackForTestTakenRequest(
        AppUserId? testTakerId,
        TierListTestTakenFeedbackData? feedbackData,
        TierListTestFeedbackOption testFeedbackOption
    ) {
        if (feedbackData is null) {
            return ErrOrNothing.Nothing;
        }

        if (testFeedbackOption is TierListTestFeedbackOption.Disabled) {
            return new Err("Feedback for this test is disabled");
        }

        if (testTakerId is null && feedbackData.LeftAnonymously == false) {
            return new Err(
                "The feedback was left non-anonymously, but the user Id is not provided",
                details: "Ensure that you are logged in"
            );
        }

        var thisFeedbackOption = testFeedbackOption as TierListTestFeedbackOption.Enabled;
        if (thisFeedbackOption is null) {
            return new Err("Invalid feedback option configuration");
        }

        if (ValidateFeedbackAnonymity(feedbackData, thisFeedbackOption.Anonymity).IsErr(out var err)) {
            return err;
        }

        if (ValidateFeedbackLength(feedbackData, thisFeedbackOption).IsErr(out err)) {
            return err;
        }

        return ErrOrNothing.Nothing;
    }
    private static ErrOrNothing ValidateFeedbackAnonymity(
        TierListTestTakenFeedbackData feedback,
        AnonymityValues thisFeedbackOptionAnonymity
    ) => (feedback.LeftAnonymously, thisFeedbackOptionAnonymity) switch {
        (LeftAnonymously: true, AnonymityValues.NonAnonymousOnly) => new Err(
            "You are trying to leave anonymous feedback, but this test only supports non-anonymous feedback"
        ),
        (LeftAnonymously: false, AnonymityValues.AnonymousOnly) => new Err(
            "You are trying to leave non-anonymous feedback, but this test only supports anonymous feedback"
        ),
        _ => ErrOrNothing.Nothing
    };

    private static ErrOrNothing ValidateFeedbackLength(
        TierListTestTakenFeedbackData feedback,
        TierListTestFeedbackOption.Enabled enabledFeedbackOption
    ) {
        int feedbackTextLen = feedback.FeedbackText?.Length ?? 0;
        if (feedbackTextLen == 0) {
            return new Err("You are trying to leave feedback, but it's empty");
        }

        if (feedbackTextLen > enabledFeedbackOption.MaxFeedbackLength) {
            return new Err(
                $"Your feedback text is too long. Maximum length of feedback is {enabledFeedbackOption.MaxFeedbackLength}",
                details: $"Current feedback text is {feedbackTextLen} characters"
            );
        }

        return ErrOrNothing.Nothing;
    }
}