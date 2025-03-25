using System.Collections.Immutable;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.formats_shared.test_styles;
using SharedKernel.Common.tests.tier_list_format;
using SharedKernel.Common.tests.tier_list_format.feedback;
using SharedKernel.Common.tests.tier_list_format.items;
using TestTakingService.Domain.TestAggregate.tier_list_format;

namespace TestTakingService.Domain.UnitTests.TestAggregateRoot.tier_list_format;

public static class TierListTestTestsConsts
{
    public static readonly AppUserId CreatorId = new(Guid.NewGuid());
    public static readonly TestId TestId = new(Guid.NewGuid());

    public static readonly TierListTestTierId TierAId = new(Guid.NewGuid());
    public static readonly TierListTestTierId TierBId = new(Guid.NewGuid());

    public static readonly TierListTestTier TierA = new(
        TierAId,
        name: "Top Tier",
        description: "The best items",
        maxItemsCountLimit: 3,
        styles: TierListTestTierStyles.Default(),
        orderInTest: 1
    );

    public static readonly TierListTestTier TierB = new(
        TierBId,
        name: "Low Tier",
        description: "Not so great",
        maxItemsCountLimit: 3,
        styles: TierListTestTierStyles.Default(),
        orderInTest: 2
    );

    public static readonly TierListTestItemId Item1Id = new(Guid.NewGuid());
    public static readonly TierListTestItemId Item2Id = new(Guid.NewGuid());
    public static readonly TierListTestItemId Item3Id = new(Guid.NewGuid());
    public static readonly TierListTestItemId Item4Id = new(Guid.NewGuid());

    public static readonly TierListTestItem Item1 = new(
        Item1Id,
        name: "Item 1",
        clarification: "First item",
        content: new TierListTestItemContentData.TextOnly("Item 1 content"),
        orderInTest: 1
    );

    public static readonly TierListTestItem Item2 = new(
        Item2Id,
        name: "Item 2",
        clarification: "Second item",
        content: new TierListTestItemContentData.TextOnly("Item 2 content"),
        orderInTest: 2
    );

    public static readonly TierListTestItem Item3 = new(
        Item3Id,
        name: "Item 3",
        clarification: "Third item",
        content: new TierListTestItemContentData.TextOnly("Item 3 content"),
        orderInTest: 3
    );

    public static readonly TierListTestItem Item4 = new(
        Item4Id,
        name: "Item 4",
        clarification: "Fourth item",
        content: new TierListTestItemContentData.TextOnly("Item 4 content"),
        orderInTest: 4
    );

    public static readonly ImmutableArray<TierListTestTier> AllTiers = [
        TierA,
        TierB
    ];

    public static readonly ImmutableArray<TierListTestItem> AllItems = [
        Item1,
        Item2,
        Item3,
        Item4
    ];

    public static TierListFormatTest CreateTest(
        AccessLevel testAccessLevel = AccessLevel.Public,
        IReadOnlyCollection<TierListTestTier> tiers = null,
        IReadOnlyCollection<TierListTestItem> items = null,
        TierListTestFeedbackOption feedbackOption = null,
        bool shuffleTiers = false,
        bool shuffleItems = false
    ) => new(
        TestId,
        CreatorId,
        [],
        testAccessLevel,
        TestStylesSheet.CreateNew(TestId),
        tiers ?? AllTiers,
        shuffleTiers: shuffleTiers,
        items ?? AllItems,
        shuffleItems: shuffleItems,
        feedbackOption ?? TierListTestFeedbackOption.Disabled.Instance
    );
}
