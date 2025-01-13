using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Common.interfaces.repositories;

public interface IGeneralFormatTestsRepository
{
    public Task AddNew(GeneralFormatTest test);
}
