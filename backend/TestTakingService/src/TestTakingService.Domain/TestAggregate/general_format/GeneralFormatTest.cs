using System.Collections.Immutable;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
using SharedKernel.Common.tests.general_format;
using SharedKernel.Common.tests.test_styles;
using TestTakingService.Domain.Common;
using TestTakingService.Domain.TestTakenRecordAggregate.general_test;

namespace TestTakingService.Domain.TestAggregate.general_format;

public class GeneralFormatTest : BaseTest
{
    private GeneralFormatTest() { }
    public override TestFormat Format => TestFormat.General;
    public IReadOnlyCollection<GeneralTestQuestion> Questions { get; init; }
    private bool _shuffleQuestions { get; init; }
    protected IReadOnlyCollection<GeneralTestResult> _results { get; init; }
    public GeneralTestFeedbackOption FeedbackOption { get; init; }
    public HashSet<GeneralTestTakenRecord> TestTakings { get; init; }

    public GeneralFormatTest(
        TestId testId,
        AppUserId creatorId,
        ImmutableHashSet<AppUserId> editors,
        AccessLevel accessLevel,
        TestStylesSheet styles,
        //general format specific
        IReadOnlyCollection<GeneralTestQuestion> questions,
        bool shuffleQuestions,
        IReadOnlyCollection<GeneralTestResult> results,
        GeneralTestFeedbackOption feedbackOption
    ) : base(testId, creatorId, editors, accessLevel, styles) {
        Questions = questions;
        _shuffleQuestions = shuffleQuestions;
        _results = results;
        FeedbackOption = feedbackOption;
    }


    // public ErrOr<GeneralTestResult> TestTaken(
    //     AppUserId? testTakerId,
    //     Dictionary<GeneralTestQuestionId, HashSet<GeneralTestAnswerId>> chosenAnswers,
    //     GeneralTestTakenFeedbackData? feedback
    // ) {
    //     if (feedback is not null) {
    //         var feedbackCheckRes = ValidateFeedbackForTestTakenRequest(testTakerId, feedback);
    //         if (feedbackCheckRes.IsErr(out var feedbackErr)) {
    //             return feedbackErr;
    //         }
    //     }
    //
    //     //check 
    //     
    //     _domainEvents.Add(new GeneralBaseTestTakenEvent());
    //     if (feedback is not null) {
    //         _domainEvents.Add(new FeedbackForGeneralTestLeftEvent());
    //     }
    // }

    private ErrOrNothing ValidateFeedbackForTestTakenRequest(
        AppUserId? testTakerId,
        GeneralTestTakenFeedbackData feedback
    ) {
        if (FeedbackOption is GeneralTestFeedbackOption.Disabled) {
            return new Err("Feedback for this test is disabled");
        }

        if (testTakerId is null && feedback.LeftAnonymously == false) {
            return new Err(
                "The feedback was left non-anonymously, but the user ID is not provided",
                details: "Ensure that you are logged in"
            );
        }

        var thisFeedbackOption = FeedbackOption as GeneralTestFeedbackOption.Enabled;
        if (thisFeedbackOption is null) {
            return new Err("Invalid feedback option configuration");
        }

        if (ValidateFeedbackAnonymity(feedback, thisFeedbackOption.Anonymity).IsErr(out var err)) {
            return err;
        }

        if (ValidateFeedbackLength(feedback, thisFeedbackOption).IsErr(out err)) {
            return err;
        }

        return ErrOrNothing.Nothing;
    }

    private ErrOrNothing ValidateFeedbackAnonymity(
        GeneralTestTakenFeedbackData feedback,
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

    private ErrOrNothing ValidateFeedbackLength(
        GeneralTestTakenFeedbackData feedback,
        GeneralTestFeedbackOption.Enabled thisFeedbackOption
    ) {
        int feedbackTextLen = feedback.FeedbackText?.Length ?? 0;
        if (feedbackTextLen == 0) {
            return new Err("You are trying to leave feedback, but it's empty");
        }

        if (feedbackTextLen > thisFeedbackOption.MaxFeedbackLength) {
            return new Err(
                $"Your feedback text is too long. Maximum length of feedback is {thisFeedbackOption.MaxFeedbackLength}",
                details: $"Current feedback text is {feedbackTextLen} characters"
            );
        }

        return ErrOrNothing.Nothing;
    }
}