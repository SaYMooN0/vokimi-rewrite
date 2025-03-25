using SharedKernel.Common.domain.entity;

namespace TestTakingService.Domain.Common.test_taken_data.tier_list_format_test;

//item id and its order in tier
public class TierListTestTakenTierData : Dictionary<TierListTestItemId, ushort>
{
    public TierListTestTakenTierData(int capacity) : base(capacity: capacity) { }
    public TierListTestTakenTierData() : base() { }

    public static TierListTestTakenTierData Parse(Dictionary<string, int> dictionary) {
        var result = new TierListTestTakenTierData(dictionary.Count);
        foreach (var kvp in dictionary) {
            result[new TierListTestItemId(new(kvp.Key))] = (ushort)kvp.Value;
        }
        return result;
    }
}