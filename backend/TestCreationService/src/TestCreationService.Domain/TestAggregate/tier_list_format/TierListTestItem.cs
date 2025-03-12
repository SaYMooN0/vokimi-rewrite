using SharedKernel.Common.domain.entity;

namespace TestCreationService.Domain.TestAggregate.tier_list_format;

public class TierListTestItem : Entity<TierListTestItemId>
{
    private TierListTestItem() { }
    public string Name { get; private set; }
    //content 
    //attributes or tags or labeld
}