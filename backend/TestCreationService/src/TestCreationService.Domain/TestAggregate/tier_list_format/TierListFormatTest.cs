using System.Collections.Immutable;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
using SharedKernel.Common.tests.tier_list_format;
using SharedKernel.Common.tests.tier_list_format.feedback;
using SharedKernel.Common.tests.tier_list_format.items;
using TestCreationService.Domain.Common;
using TestCreationService.Domain.Rules;
using TestCreationService.Domain.TestAggregate.formats_shared;
using TestCreationService.Domain.TestAggregate.formats_shared.events;

namespace TestCreationService.Domain.TestAggregate.tier_list_format;

public class TierListFormatTest : BaseTest
{
    public override TestFormat Format => TestFormat.TierList;
    private ICollection<TierListTestItem> _items { get; set; }
    private EntitiesOrderController<TierListTestTierId> _tiersOrderController { get; }
    private ICollection<TierListTestTier> _tiers { get; set; }
    private EntitiesOrderController<TierListTestItemId> _itemsOrderController { get;  }
    public TierListTestFeedbackOption Feedback { get; private set; }
    //time limit


    public static ErrOr<TierListFormatTest> CreateNew(
        AppUserId creatorId,
        string testName,
        HashSet<AppUserId> editorIds
    ) {
        var mainInfoCreation = TestMainInfo.CreateNew(testName);
        if (mainInfoCreation.IsErr(out var err)) {
            return err;
        }

        editorIds.Remove(creatorId);
        if (editorIds.Count() > TestRules.MaxTestEditorsCount) {
            return new Err(
                message: "Too many test editors",
                details:
                $"You can't add more than {TestRules.MaxTestEditorsCount} editors to the test. Current count of unique editors: {editorIds.Count()}"
            );
        }

        TierListFormatTest newTest = new(
            creatorId,
            editorIds.ToHashSet(),
            mainInfoCreation.GetSuccess()
        );
        newTest._domainEvents.Add(new TestEditorsListChangedEvent(newTest.Id, editorIds, new HashSet<AppUserId>()));
        newTest._domainEvents.Add(new NewTestInitializedEvent(newTest.Id, newTest.CreatorId));
        return newTest;
    }

    private TierListFormatTest(
        AppUserId creatorId,
        HashSet<AppUserId> editorIds,
        TestMainInfo mainInfo
    ) : base(TestId.CreateNew(), creatorId, editorIds, mainInfo) {
        TierListTestTier firstTier = TierListTestTier.CreateNew("Tier #1").GetSuccess();
        _tiers = [firstTier];
        _tiersOrderController = EntitiesOrderController<TierListTestTierId>.CreateNew(
            isShuffled: false, new() { { firstTier.Id, 1 } }
        ).GetSuccess();

        _items = [];
        _itemsOrderController = EntitiesOrderController<TierListTestItemId>.Empty(isShuffled: false);
    }

    public override void DeleteTest() {
        throw new NotImplementedException();
        // _domainEvents.Add(new TierListFormatTestDeletedEvent(Id, CreatorId, _editorIds));
    }

    public ImmutableArray<(TierListTestTier, ushort)> TiersWithOrder => _tiersOrderController
        .GetItemsWithOrders(_tiers)
        .ToImmutableArray();

    public ImmutableArray<(TierListTestItem, ushort)> ItemsWithOrder => _itemsOrderController
        .GetItemsWithOrders(_items)
        .ToImmutableArray();

    public ErrOr<TierListTestTier> AddNewTier(string tierName) {
        if (_tiers.Count >= TierListTestRules.TestMaxTiersCount) {
            return new Err(
                $"Cannot add more than {TierListTestRules.TestMaxTiersCount} tiers to the test",
                details: $"Current tiers count is {_tiers.Count}"
            );
        }

        var nameAlreadyExists = _tiers.Select(t => t.Name).Contains(tierName);
        if (nameAlreadyExists) {
            return new Err("Tier with this name already exists in this test");
        }

        var creationRes = TierListTestTier.CreateNew(tierName);
        if (creationRes.IsErr(out var err)) {
            return err;
        }

        _tiers.Add(creationRes.GetSuccess());
        return creationRes.GetSuccess();
    }

    public ErrOr<TierListTestItem> AddNewItem(
        string newItemName,
        string? newItemClarification,
        TierListTestItemContentData newItemContent
    ) {
        if (_items.Count >= TierListTestRules.TestMaxItemsCount) {
            return new Err(
                $"Cannot add more than {TierListTestRules.TestMaxItemsCount} items to the test",
                details: $"Current items count is {_items.Count}"
            );
        }

        var nameAlreadyExists = _items.Select(item => item.Name).Contains(newItemName);
        if (nameAlreadyExists) {
            return new Err("Item with this name already exists in this test");
        }

        ErrOr<TierListTestItem> creationRes = TierListTestItem.CreateNew(
            newItemName, newItemClarification, newItemContent
        );
        if (creationRes.IsErr(out var err)) {
            return err;
        }

        _items.Add(creationRes.GetSuccess());
        return creationRes.GetSuccess();
    }

    public ErrOr<TierListTestItem> UpdateItem(
        TierListTestItemId itemId,
        string newName,
        string? newClarification,
        TierListTestItemContentData newContent
    ) {
        TierListTestItem? itemToUpdate = _items.FirstOrDefault(item => item.Id == itemId);
        if (itemToUpdate is null) {
            return Err.ErrFactory.NotFound(
                "Cannot update item because it wasn't found in this test",
                details: $"This test has no item with id {itemId} ");
        }

        var updateRes = itemToUpdate.Update(newName, newClarification, newContent);
        if (updateRes.IsErr(out var err)) {
            return err;
        }

        return itemToUpdate;
    }

    public void UpdateTestFeedback(TierListTestFeedbackOption testFeedback) =>
        Feedback = testFeedback;
}