using SharedKernel.Common.tests.tier_list_format.items;
using TestTakingService.Domain.TestAggregate.tier_list_format;

namespace TestTakingService.Api.Contracts.load_test_taking_data.tier_list_test;

internal record class TierListTestTakingItemData(
    string Id,
    ushort Order,
    string Name,
    string? Clarification,
    Dictionary<string, string> Content,
    TierListTestItemContentType ContentType
)
{
    public static TierListTestTakingItemData FromItem(TierListTestItem item) => new(
        item.Id.ToString(),
        item.OrderInTest,
        item.Name,
        item.Clarification,
        item.Content.ToDictionary(),
        item.Content.MatchingEnumType
    );
}