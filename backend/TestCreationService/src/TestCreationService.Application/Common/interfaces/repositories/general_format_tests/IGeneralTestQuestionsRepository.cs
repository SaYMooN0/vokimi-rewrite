using SharedKernel.Common.EntityIds;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Common.interfaces.repositories.general_format_tests;

public interface IGeneralTestQuestionsRepository
{
    public Task<GeneralTestQuestion?> GetById(GeneralTestQuestionId id);
    public Task<GeneralTestQuestion?> GetWithAnswers(GeneralTestQuestionId id);
    public Task Update(GeneralTestQuestion question);
}
