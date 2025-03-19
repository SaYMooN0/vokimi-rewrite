using TestCatalogService.Domain.TestAggregate.tier_list_format;

namespace TestCatalogService.Domain.Common.interfaces.repositories.tests;

public interface ITierListFormatTestsRepository
{
    public Task Add(TierListFormatTest tierListTest);
}