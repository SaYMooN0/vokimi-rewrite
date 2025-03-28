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
using TestTakingService.Domain.TestAggregate.tier_list_format.helpers;
using TestTakingService.Domain.TestFeedbackRecordAggregate.events;
using TestTakingService.Domain.TestTakenRecordAggregate.events;

namespace TestTakingService.Domain.TestAggregate.tier_list_format;

public class TierListFormatTest : BaseTest
{
    private TierListFormatTest() { }
    public override TestFormat Format => TestFormat.TierList;
    public IReadOnlyCollection<TierListTestItem> Items { get; init; }
    private bool _shuffleItems { get; }
    public IReadOnlyCollection<TierListTestTier> Tiers { get; init; }
    private bool _shuffleTiers { get; }
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

        HashSet<TierListTestItemId> providedItemIds = [];
        foreach (var tier in this.Tiers) {
            if (!itemsInTiers.TryGetValue(tier.Id, out var tierData)) {
                return Err.ErrFactory.InvalidData(
                    "Data about items is not provided for all the tiers",
                    details: $"Tier with id '{tier.Id}' has no items data"
                );
            }

            if (tierData.Count > tier.MaxItemsCountLimit) {
                return Err.ErrFactory.InvalidData(
                    $"Items count limit in tier '{tier.Name}' is exceeded",
                    details:
                    $"Maximum allowed items in tier '{tier.MaxItemsCountLimit}', current items count is '{tierData.Count}'"
                );
            }

            var sortedByOrder = tierData.OrderBy(
                (iWId) => iWId.Value
            );
            ushort expectedOrder = 0;
            foreach (var (providedItemId, itemOrder)in sortedByOrder) {
                TierListTestItem? itemInTest = this.Items.FirstOrDefault(i => i.Id == providedItemId);
                if (itemInTest is null) {
                    return Err.ErrFactory.InvalidData(
                        "One of the provided items is not presented in this test",
                        details: $"Unexpected item id'{providedItemId}'"
                    );
                }

                if (itemOrder != expectedOrder) {
                    return Err.ErrFactory.InvalidData(
                        $"Provided item order for item {itemInTest.Name} is invalid",
                        details: $"Expected item order: {expectedOrder}, provided item order: {itemOrder}'"
                    );
                }

                if (!providedItemIds.Add(providedItemId)) {
                    return Err.ErrFactory.InvalidData(
                        $"Item {itemInTest.Name} was provided in two or more different tiers"
                    );
                }
            }
        }

        if (providedItemIds.Count != Items.Count) {
            return Err.ErrFactory.InvalidData(
                $"Test has '{Items.Count} items. {providedItemIds.Count} items were provided",
                "Incorrect count of provided items"
            );
        }

        CreateTestTakenEvent(testTakerId, testTakingStart, testTakingEnd, itemsInTiers);

        if (feedback is not null) {
            _domainEvents.Add(new FeedbackForTierListTestLeftEvent(
                Id, testTakerId, testTakingEnd, feedback.FeedbackText, feedback.LeftAnonymously
            ));
        }

        return itemsInTiers;
    }

    private void CreateTestTakenEvent(
        AppUserId? testTakerId,
        DateTime testTakingStart,
        DateTime testTakingEnd,
        Dictionary<TierListTestTierId, TierListTestTakenTierData> itemsInTiers
    ) {
        var testTakenRecordId = TestTakenRecordId.CreateNew();

        if (testTakerId is not null) {
            _takenByUserIds.Add(testTakerId);
        }

        _testTakenRecordIds.Add(testTakenRecordId);

        var testTakenEvent = new TierListTestTakenEvent(
            testTakenRecordId, Id, testTakerId,
            testTakingStart, testTakingEnd,
            itemsInTiers
        );
        _domainEvents.Add(testTakenEvent);
    }
}