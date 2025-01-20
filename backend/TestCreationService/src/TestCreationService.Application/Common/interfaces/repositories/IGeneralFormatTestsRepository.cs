using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Common.interfaces.repositories;

public interface IGeneralFormatTestsRepository
{
    public Task AddNew(GeneralFormatTest test);
    public Task<GeneralFormatTest?> GetById(TestId testId);
    public Task<GeneralFormatTest?> GetWithQuestions(TestId testId);
    public Task Update(GeneralFormatTest test);
}
