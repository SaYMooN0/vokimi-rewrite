using TestManagingService.Domain.TestAggregate.general_format;

namespace TestManagingService.Application.Common.interfaces.repositories.tests;

public interface IGeneralFormatTestsRepository
{
    public Task Add(GeneralFormatTest generalTest);
}