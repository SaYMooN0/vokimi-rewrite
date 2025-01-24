using SharedKernel.Common.EntityIds;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Common.interfaces.repositories.general_format_tests;

public interface IGeneralTestAnswersRepository
{
    public Task<GeneralTestAnswer?> GetById(GeneralTestAnswerId id);
    public Task Update(GeneralTestAnswer question);
}
