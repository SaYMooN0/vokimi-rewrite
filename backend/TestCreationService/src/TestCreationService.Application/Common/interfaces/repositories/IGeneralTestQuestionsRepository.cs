using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using System.Collections.Immutable;
using TestCreationService.Domain.GeneralTestQuestionAggregate;

namespace TestCreationService.Application.Common.interfaces.repositories;

public interface IGeneralTestQuestionsRepository
{
    public Task<GeneralTestQuestion?> GetById(GeneralTestQuestionId id);
    public Task<ErrOr<IEnumerable<GeneralTestQuestion>>> GetAllWithId(IEnumerable<GeneralTestQuestionId> ids);
    public Task<GeneralTestQuestion?> GetWithAnswers(GeneralTestQuestionId id);
    public Task Update(GeneralTestQuestion question);
}
