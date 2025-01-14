using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Infrastructure.Persistence.repositories;

internal class GeneralFormatTestsRepository : IGeneralFormatTestsRepository
{
    public Task AddNew(GeneralFormatTest test) {
        throw new NotImplementedException();
    }
}
