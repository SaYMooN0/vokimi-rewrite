using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.tier_list_format;

namespace TestCreationService.Domain.TestAggregate.tier_list_format;

public class TierListTestItem : Entity<TierListTestItemId>
{
    private TierListTestItem() { }
    public string Name { get; private set; }

    public string? Clarification { get; private set; }
    public TierListTestItemContentData Content { get; private set; }
}