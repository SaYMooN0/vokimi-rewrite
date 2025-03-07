using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Application.Common.interfaces.repositories.tests;

public interface IGeneralFormatTestsRepository
{
    public Task Add(GeneralFormatTest test);
    public Task Update(GeneralFormatTest test);
    public Task<GeneralFormatTest?> GetById(TestId testId);
    public Task<GeneralFormatTest?> GetWithQuestionWithAnswers(TestId testId);
    public Task<GeneralFormatTest?> GetWithQuestionsAnswersAndResults(TestId testId);
    public Task<GeneralFormatTest?> GetWithResults(TestId testId);
}