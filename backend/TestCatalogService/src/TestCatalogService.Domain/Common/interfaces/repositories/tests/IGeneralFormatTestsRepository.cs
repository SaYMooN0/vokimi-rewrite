using TestCatalogService.Domain.TestAggregate.general_format;

namespace TestCatalogService.Application.Common.interfaces.repositories.tests;

public interface IGeneralFormatTestsRepository
{
    public Task Add(GeneralFormatTest generalTest);
}