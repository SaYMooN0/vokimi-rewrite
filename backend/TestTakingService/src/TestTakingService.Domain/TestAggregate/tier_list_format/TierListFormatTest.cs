using System.Collections.Immutable;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using SharedKernel.Common.tests;
using SharedKernel.Common.tests.formats_shared.test_styles;
using SharedKernel.Common.tests.tier_list_format.feedback;
using TestTakingService.Domain.Common.test_taken_data.tier_list_format_test;

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
        AppUserId? requestTestTakerId,
        Dictionary<TierListTestTierId, TierListTestTakenTierData> itemsInTiers,
        DateTime testTakingStart,
        DateTime testTakingEnd,
        TierListTestTakenFeedbackData? feedbackData,
        IDateTimeProvider dateTimeProvider
    ) {
        
        //     _domainEvents.Add(new FeedbackForTierListTestLeftEvent(
        //         Id, testTakerId, testTakingEnd, feedback.FeedbackText, feedback.LeftAnonymously
        //     ));
    }
}