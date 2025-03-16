using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.items;

public record class TierListTestItemInfoResponse(
    string Id,
    string Name,
    string? Clarification,
    Dictionary<string, string> Content
)
{
    public static TierListTestItemInfoResponse FromItem(TierListTestItem item) => new(
    );
}