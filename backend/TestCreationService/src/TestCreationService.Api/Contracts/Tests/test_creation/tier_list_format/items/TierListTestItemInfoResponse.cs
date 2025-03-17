using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.items;

internal record class TierListTestItemInfoResponse(
    string Id,
    string Name,
    string? Clarification,
    Dictionary<string, string> Content
)
{
    public static TierListTestItemInfoResponse FromItem(TierListTestItem item) => new(
        item.Id.ToString(),
        item.Name,
        item.Clarification,
        item.Content.ToDictionary()
    );
}