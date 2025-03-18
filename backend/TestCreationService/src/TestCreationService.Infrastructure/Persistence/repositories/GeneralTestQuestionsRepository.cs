using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.GeneralTestQuestionAggregate;

namespace TestCreationService.Infrastructure.Persistence.repositories;

internal class GeneralTestQuestionsRepository : IGeneralTestQuestionsRepository
{
    private readonly TestCreationDbContext _db;

    public GeneralTestQuestionsRepository(TestCreationDbContext db) {
        _db = db;
    }

    public async Task Add(GeneralTestQuestion question) {
        await _db.GeneralTestQuestions.AddAsync(question);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteById(GeneralTestQuestionId questionId) {
        await _db.GeneralTestQuestions
            .Where(q => q.Id == questionId)
            .ExecuteDeleteAsync();
    }

    public async Task<ImmutableArray<GeneralTestQuestion>> GetWithAnswersByIds(HashSet<GeneralTestQuestionId> ids) =>
        (await _db.GeneralTestQuestions
            .Where(q => ids.Contains(q.Id))
            .Include(q => EF.Property<List<GeneralTestQuestion>>(q, "_answers"))
            .ToArrayAsync()
        ).ToImmutableArray();
    public async Task<ErrOr<List<GeneralTestQuestion>>> GetAllWithIdWithoutAnswers(IEnumerable<GeneralTestQuestionId> ids) {
        List<GeneralTestQuestion> result = new(ids.Count());
        foreach (var id in ids) {
            GeneralTestQuestion? q = await _db.GeneralTestQuestions.FindAsync(id);
            if (q is null) {
                return Err.ErrFactory.NotFound($"Unable to get all the questions. Question with id {id} not found");
            }
            result.Add(q);
        }
        return result;
    }

    public async Task<GeneralTestQuestion?> GetById(GeneralTestQuestionId id) =>
        await _db.GeneralTestQuestions.FindAsync(id);

    public async Task<GeneralTestQuestion?> GetWithAnswers(GeneralTestQuestionId id) =>
        await _db.GeneralTestQuestions
            .Include(q => EF.Property<List<GeneralTestQuestion>>(q, "_answers"))
            .FirstOrDefaultAsync(q => q.Id == id);

    public async Task Update(GeneralTestQuestion question) {
        _db.GeneralTestQuestions.Update(question);
        await _db.SaveChangesAsync();
    }
}