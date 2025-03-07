using SharedKernel.Common.domain.entity;
using TestManagingService.Domain.TestAggregate.general_format;

namespace TestManagingService.Application.Common.interfaces.repositories.tests;

public interface IGeneralFormatTestsRepository
{
    public Task<GeneralFormatTest?> GetById(TestId testId);
    public Task Add(GeneralFormatTest generalTest);
    public Task Update(GeneralFormatTest test);
}