namespace TestTakingService.Api.Contracts.test_taken.tier_list_test;

public record class TierListTestTakenResponse(
    Dictionary<TierListTestTierId, TierListTestTakenTierData> FinalTierList
    )
{
    
}