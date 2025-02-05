using TestTakingService.Domain.TestTagAggregate.general_format;

namespace TestTakingService.Application.Common.interfaces.repositories;

public interface IGeneralFormatTestsRepository
{
    public Task Add(GeneralFormatTest test);

}
