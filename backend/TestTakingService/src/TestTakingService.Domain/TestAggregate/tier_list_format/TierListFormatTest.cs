using System.Collections.Immutable;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using SharedKernel.Common.tests;
using SharedKernel.Common.tests.formats_shared.test_styles;
using SharedKernel.Common.tests.tier_list_format.feedback;
using TestTakingService.Domain.Common;
using TestTakingService.Domain.Common.test_taken_data.tier_list_format_test;
using TestTakingService.Domain.TestAggregate.general_format;
using TestTakingService.Domain.TestAggregate.general_format.helpers;
using TestTakingService.Domain.TestAggregate.tier_list_format.helpers;
using TestTakingService.Domain.TestFeedbackRecordAggregate.events;
using TestTakingService.Domain.TestTakenRecordAggregate.events;

namespace TestTakingService.Domain.TestAggregate.tier_list_format;

public class TierListFormatTest : BaseTest
{
    private TierListFormatTest() { }
    public override TestFormat Format => TestFormat.TierList;
    public IReadOnlyCollection<TierListTestTier> Tiers { get; init; }
    private bool _shuffleTiers { get; }
    public IReadOnlyCollection<TierListTestItem> Items { get; init; }
    private bool _shuffleItems { get; }
    public TierListTestFeedbackOption FeedbackOption { get; private set; }

    public TierListFormatTest(
        TestId testId,
        AppUserId creatorId,
        ImmutableHashSet<AppUserId> editors,
        AccessLevel accessLevel,
        TestStylesSheet styles,
        //tier list format specific
        IReadOnlyCollection<TierListTestTier> tiers,
        bool shuffleTiers,
        IReadOnlyCollection<TierListTestItem> items,
        bool shuffleItems,
        TierListTestFeedbackOption feedbackOption
    ) : base(testId, creatorId, editors, accessLevel, styles) {
        Tiers = tiers;
        _shuffleTiers = shuffleTiers;
        Items = items;
        _shuffleItems = shuffleItems;
        FeedbackOption = feedbackOption;
    }

    public void UpdateFeedbackOption(TierListTestFeedbackOption newFeedbackOption) {
        FeedbackOption = newFeedbackOption;
    }

    public ErrOr<Dictionary<TierListTestTierId, TierListTestTakenTierData>> TestTaken(
        AppUserId? testTakerId,
        Dictionary<TierListTestTierId, TierListTestTakenTierData> itemsInTiers,
        DateTime testTakingStart,
        DateTime testTakingEnd,
        TierListTestTakenFeedbackData? feedback,
        IDateTimeProvider dateTimeProvider
    ) {
        if (
            TierListTestTakingValidatorHelper.CheckForTestTimeDurationErrs(
                dateTimeProvider,
                testStartTime: testTakingStart,
                testEndTime: testTakingEnd
            ).IsErr(out var timeErr)
        ) {
            return timeErr;
        }

        if (
            TierListTestTakingValidatorHelper
            .ValidatePossibleFeedbackForTestTakenRequest(testTakerId, feedback, FeedbackOption)
            .IsErr(out var feedbackErr)
        ) {
            return feedbackErr;
        }

        ..//validate items in tiers order + items in tier limits

        return receivedRes;
    }
}