using SharedKernel.Common.domain;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Application.Common.interfaces.repositories.tests;

public interface IGeneralFormatTestsRepository
{
    public Task Add(GeneralFormatTest test);
    public Task<GeneralFormatTest?> GetWithQuestionWithAnswers(TestId testId);
    public Task<GeneralFormatTest?> GetWithQuestionsAnswersAndResults(TestId testId);
    public Task<GeneralFormatTest?> GetWithResults(TestId testId);
}