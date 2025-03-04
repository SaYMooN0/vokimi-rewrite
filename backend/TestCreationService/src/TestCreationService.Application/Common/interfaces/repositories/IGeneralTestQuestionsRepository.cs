using System.Collections.Immutable;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Domain.GeneralTestQuestionAggregate;

namespace TestCreationService.Application.Common.interfaces.repositories;

public interface IGeneralTestQuestionsRepository
{
    public Task<GeneralTestQuestion?> GetById(GeneralTestQuestionId id);
    public Task<ErrOr<IEnumerable<GeneralTestQuestion>>> GetAllWithId(IEnumerable<GeneralTestQuestionId> ids);
    public Task<GeneralTestQuestion?> GetWithAnswers(GeneralTestQuestionId id);
    public Task Update(GeneralTestQuestion question);
    public Task DeleteById(GeneralTestQuestionId questionId);
    public Task Add(GeneralTestQuestion question);
}
