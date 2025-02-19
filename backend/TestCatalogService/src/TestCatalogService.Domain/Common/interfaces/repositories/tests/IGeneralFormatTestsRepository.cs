using TestCatalogService.Domain.TestAggregate.general_format;

namespace TestCatalogService.Domain.Common.interfaces.repositories.tests;

public interface IGeneralFormatTestsRepository
{
    public Task Add(GeneralFormatTest generalTest);
}