using System.Collections.Immutable;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using SharedKernel.Common.tests;
using SharedKernel.Common.tests.formats_shared.interaction_access_settings;
using SharedKernel.Common.tests.formats_shared.test_styles;
using SharedKernel.Common.tests.general_format;
using TestTakingService.Domain.Common;
using TestTakingService.Domain.Common.test_taken_data.general_format_test;
using TestTakingService.Domain.TestAggregate.general_format.helpers;
using TestTakingService.Domain.TestFeedbackRecordAggregate.events;
using TestTakingService.Domain.TestTakenRecordAggregate.events;
using TestTakingService.Domain.TestTakenRecordAggregate.general_test;

namespace TestTakingService.Domain.TestAggregate.general_format;

public class GeneralFormatTest : BaseTest
{
    private GeneralFormatTest() { }
    public override TestFormat Format => TestFormat.General;
    public IReadOnlyCollection<GeneralTestQuestion> Questions { get; init; }
    private bool _shuffleQuestions { get; }
    public IReadOnlyCollection<GeneralTestResult> Results { get; init; }
    public GeneralTestFeedbackOption FeedbackOption { get; private set; }

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
        Results = results;
        FeedbackOption = feedbackOption;
    }

    public ErrOr<GeneralTestResult> TestTaken(
        AppUserId? testTakerId,
        Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> questionsDataMap,
        DateTime testTakingStart,
        DateTime testTakingEnd,
        GeneralTestTakenFeedbackData? feedback,
        IDateTimeProvider dateTimeProvider
    ) {
        if (
            GeneralTestTakingValidatorHelper.CheckForTestTimeDurationErrs(
                dateTimeProvider,
                questionsDataMap.Values.Select(q => q.TimeOnQuestionSpent),
                testStartTime: testTakingStart,
                testEndTime: testTakingEnd
            ).IsErr(out var timeErr)
        ) {
            return timeErr;
        }

        if (
            GeneralTestTakingValidatorHelper
            .ValidatePossibleFeedbackForTestTakenRequest(testTakerId, feedback, FeedbackOption)
            .IsErr(out var feedbackErr)
        ) {
            return feedbackErr;
        }

        Dictionary<GeneralTestQuestionId, GeneralTestTakenEventQuestionDetails> testTakenQuestionDetails = [];
        Dictionary<GeneralTestResultId, ushort> resultsWithPoints = [];

        foreach (var testQuestion in this.Questions) {
            var questionDataRes =
                GeneralTestTakingValidatorHelper.GetValidQuestionData(questionsDataMap, testQuestion);
            if (questionDataRes.IsErr(out var err)) {
                return err;
            }

            var questionData = questionDataRes.GetSuccess();
            foreach (var qChosenAnswerId in questionData.ChosenAnswers) {
                var resultIdsToAddPoints = testQuestion.Answers
                    .First(a => a.Id == qChosenAnswerId).RelatedResultIds;
                foreach (var resId in resultIdsToAddPoints) {
                    resultsWithPoints[resId] = (ushort)(resultsWithPoints.TryGetValue(resId, out var points)
                            ? points + 1
                            : 1
                        );
                }
            }

            testTakenQuestionDetails.Add(testQuestion.Id, new GeneralTestTakenEventQuestionDetails(
                questionData.ChosenAnswers, questionData.TimeOnQuestionSpent
            ));
        }

        GeneralTestResult receivedRes = GetResultWithMaxPoints(resultsWithPoints);

        var testTakenRecordId = TestTakenRecordId.CreateNew();
        CreateTestTakenEvent(
            testTakerId, testTakingStart, testTakingEnd,
            testTakenRecordId, receivedRes.Id, testTakenQuestionDetails
        );
        if (feedback is not null) {
            _domainEvents.Add(new FeedbackForGeneralTestLeftEvent(
                Id, testTakerId, testTakingEnd, feedback.FeedbackText, feedback.LeftAnonymously
            ));
        }

        return receivedRes;
    }

    private void CreateTestTakenEvent(
        AppUserId? testTakerId,
        DateTime testTakingStart,
        DateTime testTakingEnd,
        TestTakenRecordId testTakenRecordId,
        GeneralTestResultId receivedResId,
        Dictionary<GeneralTestQuestionId, GeneralTestTakenEventQuestionDetails> testTakenQuestionDetails
    ) {
        if (testTakerId is not null) {
            _takenByUserIds.Add(testTakerId);
        }

        _testTakenRecordIds.Add(testTakenRecordId);

        var testTakenEvent = new GeneralTestTakenEvent(
            testTakenRecordId, Id, testTakerId,
            testTakingStart, testTakingEnd,
            receivedResId, testTakenQuestionDetails
        );
        _domainEvents.Add(testTakenEvent);
    }

    private GeneralTestResult GetResultWithMaxPoints(Dictionary<GeneralTestResultId, ushort> resultsWithPoints) {
        ushort maxPoints = 0;
        GeneralTestResultId resToReceiveId = Results.First().Id;

        foreach (var (resId, points) in resultsWithPoints) {
            if (points > maxPoints) {
                maxPoints = points;
                resToReceiveId = resId;
            }
        }

        return Results.First(r => r.Id == resToReceiveId);
    }

    public void UpdateFeedbackOption(GeneralTestFeedbackOption newFeedbackOption) {
        FeedbackOption = newFeedbackOption;
    }
}