using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
using TestCreationService.Domain.Common;
using TestCreationService.Domain.Rules;
using TestCreationService.Domain.TestAggregate.formats_shared;
using TestCreationService.Domain.TestAggregate.formats_shared.events;

namespace TestCreationService.Domain.TestAggregate.tier_list_format;

public class TierListFormatTest : BaseTest
{
    public override TestFormat Format => TestFormat.TierList;
    private ICollection<TierListTestItem> _items { get; set; }
    private EntitiesOrderController<TierListTestTierId> _tiersOrderController { get; set; }
    private ICollection<TierListTestTier> _tiers { get; set; }
    private EntitiesOrderController<TierListTestItemId> _itemsOrderController { get; set; }
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
        TierListTestTier firstTier = TierListTestTier.CreateNew().GetSuccess();
        _tiers = [firstTier];
        _tiersOrderController = EntitiesOrderController<TierListTestTierId>.CreateNew(
            isShuffled: false, new() { { firstTier.Id, 1 } }
        ).GetSuccess();

        _items = [];
        _itemsOrderController = EntitiesOrderController<TierListTestItemId>.Empty(isShuffled: false);
    }

    public override void DeleteTest() {
        // _domainEvents.Add(new TierListFormatTestDeletedEvent(Id, CreatorId, _editorIds));
    }
}