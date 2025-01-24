using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.EntityIds;
using TestCreationService.Application.Common.interfaces.repositories.general_format_tests;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Infrastructure.Persistence.repositories.general_format_tests;

internal class GeneralTestQuestionsRepository : IGeneralTestQuestionsRepository
{
    private readonly TestCreationDbContext _db;

    public GeneralTestQuestionsRepository(TestCreationDbContext db) {
        _db = db;
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
