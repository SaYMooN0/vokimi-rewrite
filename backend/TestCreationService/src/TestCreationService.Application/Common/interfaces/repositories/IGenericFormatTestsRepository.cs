using TestCreationService.Domain.TestAggregate.generic_format;

namespace TestCreationService.Application.Common.interfaces.repositories;

public interface IGenericFormatTestsRepository
{
    public Task AddNew(GenericFormatTest test);
}
