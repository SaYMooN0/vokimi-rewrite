using ApiShared.interfaces;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestTakingService.Domain.Common.test_taken_data.tier_list_format_test;

namespace TestTakingService.Api.Contracts.test_taken.tier_list_test;

internal class TierListTestTakenRequest : TestTakenRequest
{
    //tier id + item with their order in it
    public Dictionary<string, Dictionary<string, int>> ItemsInTiers { get; init; } = [];
    public TierListTestTakenFeedbackData? Feedback { get; init; }
    public override DateTime StartDateTime { get; init; }
    public override DateTime EndDateTime { get; init; }

    private const int
        _maxTiersDataCount = 500,
        _maxItemsInTierCount = 500;

    public Dictionary<TierListTestTierId, TierListTestTakenTierData> ParsedItemsInTiers =>
        ItemsInTiers.ToDictionary(
            tier => new TierListTestTierId(new(tier.Key)),
            tier => TierListTestTakenTierData.Parse(tier.Value)
        );

    
    public override RequestValidationResult Validate() {
        if (ItemsInTiers.Count == 0) {
            return new ErrList(Err.ErrFactory.InvalidData(
                "Info about items in tiers is not provided"
            ));
        }

        if (ItemsInTiers.Count > _maxTiersDataCount) {
            return new ErrList(Err.ErrFactory.InvalidData(
                "Data for too many tiers is provided"
            ));
        }

        ErrList errs = new();
        errs.AddPossibleErr(base.ValidateStartAndEndDateTine());

        foreach (var (tierId, itemsInTier) in ItemsInTiers) {
            if (!Guid.TryParse(tierId, out _)) {
                errs.Add(Err.ErrFactory.InvalidData(
                    "Unable to parse test tier id",
                    $"Incorrect tier id format: {tierId}"
                ));
                continue;
            }

            errs.AddRange(CheckItemsInTierForErr(tierId, itemsInTier));
        }

        return errs;
    }

    private IEnumerable<Err> CheckItemsInTierForErr(string tierId, Dictionary<string, int> itemsInTier) {
        if (itemsInTier.Count > _maxItemsInTierCount) {
            yield return Err.ErrFactory.InvalidData(
                "Too many items for one tier selected",
                $"Tier id: {tierId}, items in tier count: {itemsInTier.Count}"
            );
            yield break;
        }

        int expectedOrder = 0;
        var itemsSortedByOrder = itemsInTier.OrderBy(pair => pair.Value);

        foreach (var (itemId, order) in itemsSortedByOrder) {
            if (!Guid.TryParse(itemId, out _)) {
                yield return Err.ErrFactory.InvalidData(
                    "Unable to parse test item id",
                    $"Incorrect item id format: {itemId}"
                );
            }

            if (order != expectedOrder) {
                yield return Err.ErrFactory.InvalidData(
                    "Incorrect item order in tier",
                    $"Item id {itemId} has order {order}, expected order {expectedOrder}"
                );
            }

            if (order < ushort.MinValue || order > ushort.MaxValue) {
                yield return Err.ErrFactory.InvalidData(
                    "Incorrect item order in tier",
                    $"Tier id: {tierId}, order {order} is out of range"
                );
            }

            expectedOrder++;
        }
    }
}