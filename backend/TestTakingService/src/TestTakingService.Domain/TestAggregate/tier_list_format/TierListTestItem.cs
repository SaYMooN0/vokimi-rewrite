using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.tier_list_format.items;

namespace TestTakingService.Domain.TestAggregate.tier_list_format;

public class TierListTestItem : Entity<TierListTestItemId>
{
    private TierListTestItem() { }
    public string Name { get; }

    public string? Clarification { get; }
    public TierListTestItemContentData Content { get; }

    public ushort OrderInTest { get; }

    public TierListTestItem(
        TierListTestItemId id,
        string name,
        string? clarification,
        TierListTestItemContentData content,
        ushort orderInTest
    ) {
        Id = id;
        Name = name;
        Clarification = clarification;
        Content = content;
        OrderInTest = orderInTest;
    }
}