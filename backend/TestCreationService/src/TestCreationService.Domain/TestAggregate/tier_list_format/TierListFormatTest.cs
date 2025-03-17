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
    private EntitiesOrderController<TierListTestItemId> _itemsOrderController { get; set; }
    private ICollection<TierListTestTier> _tiers { get; set; }
    private EntitiesOrderController<TierListTestTierId> _tiersOrderController { get; set; }
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
        _items = [];
        _itemsOrderController = EntitiesOrderController<TierListTestItemId>.Empty(isShuffled: false);

        TierListTestTier firstTier = TierListTestTier.CreateNew("Tier #1").GetSuccess();
        _tiers = [firstTier];
        _tiersOrderController = EntitiesOrderController<TierListTestTierId>.CreateNew(
            isShuffled: false, new() { { firstTier.Id, 1 } }
        ).GetSuccess();

        Feedback = TierListTestFeedbackOption.Disabled.Instance;
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

        var newTier = creationRes.GetSuccess();
        _tiers.Add(newTier);
        _tiersOrderController.AddToEnd(newTier.Id);
        return newTier;
    }

    public ErrOr<TierListTestTier> UpdateTier(
        TierListTestTierId tierId,
        string newTierName,
        string? newTierDescription,
        ushort? newMaxItemsCountLimit,
        TierListTestTierStyles newStyles
    ) {
        TierListTestTier? tierToUpdate = _tiers.FirstOrDefault(item => item.Id == tierId);
        if (tierToUpdate is null) {
            return Err.ErrFactory.NotFound(
                "Cannot update tier because it wasn't found in this test",
                details: $"This test has no tier with id {tierId} "
            );
        }

        var updateRes = tierToUpdate.Update(
            newTierName, newTierDescription, newMaxItemsCountLimit, newStyles
        );
        if (updateRes.IsErr(out var err)) {
            return err;
        }

        return tierToUpdate;
    }

    public ErrOrNothing UpdateTiersOrder(EntitiesOrderController<TierListTestTierId> orderController) {
        var tierIds = _tiersOrderController.EntityIds();
        var providedIds = orderController.EntityIds().ToHashSet();

        var extraIds = providedIds.Except(tierIds);
        if (extraIds.Any()) {
            return Err.ErrFactory.InvalidData(
                "Invalid tiers order was provided. Some tiers do not exist in the test",
                details: $"Extra tier Ids: {string.Join(", ", extraIds)}"
            );
        }

        if (!tierIds.IsSubsetOf(providedIds)) {
            var missingIds = tierIds.Except(providedIds);
            return Err.ErrFactory.InvalidData(
                "Invalid tiers order was provided. Not all tiers presented",
                details: $"Missing tier Ids: {string.Join(", ", missingIds)}"
            );
        }

        _tiersOrderController = orderController.Copy();
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing RemoveTier(TierListTestTierId tierId) {
        var tier = _tiers.FirstOrDefault(q => q.Id == tierId);
        if (tier is null) {
            return Err.ErrFactory.NotFound(
                message: "Cannot find the tier to remove",
                details:
                $"Tier list test with id {Id} has no tier with id {tierId}. Either it has already been removed or it was never in the test"
            );
        }

        _tiers.Remove(tier);
        _tiersOrderController.RemoveEntity(tier.Id);
        return ErrOrNothing.Nothing;
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

        var newItem = creationRes.GetSuccess();
        _items.Add(newItem);
        _itemsOrderController.AddToEnd(newItem.Id);
        return newItem;
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

    public ErrOrNothing UpdateItemsOrder(
        EntitiesOrderController<TierListTestItemId> orderController
    ) {
        var itemIds = _itemsOrderController.EntityIds();
        var providedIds = orderController.EntityIds().ToHashSet();

        var extraIds = providedIds.Except(itemIds);
        if (extraIds.Any()) {
            return Err.ErrFactory.InvalidData(
                "Invalid items order was provided. Some items do not exist in the test",
                details: $"Extra item Ids: {string.Join(", ", extraIds)}"
            );
        }

        if (!itemIds.IsSubsetOf(providedIds)) {
            var missingIds = itemIds.Except(providedIds);
            return Err.ErrFactory.InvalidData(
                "Invalid items order was provided. Not all items presented",
                details: $"Missing item Ids: {string.Join(", ", missingIds)}"
            );
        }

        _itemsOrderController = orderController.Copy();
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing RemoveItem(TierListTestItemId itemId) {
        var item = _items.FirstOrDefault(q => q.Id == itemId);
        if (item is null) {
            return Err.ErrFactory.NotFound(
                message: "Cannot find the item to remove",
                details:
                $"Tier list test with id {Id} has no item with id {itemId}. Either it has already been removed or it was never in the test"
            );
        }

        _items.Remove(item);
        _itemsOrderController.RemoveEntity(item.Id);
        return ErrOrNothing.Nothing;
    }

    public void UpdateTestFeedback(TierListTestFeedbackOption testFeedback) =>
        Feedback = testFeedback;
}