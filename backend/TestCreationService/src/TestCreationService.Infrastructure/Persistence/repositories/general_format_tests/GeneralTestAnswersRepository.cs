
using SharedKernel.Common.EntityIds;
using TestCreationService.Application.Common.interfaces.repositories.general_format_tests;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Infrastructure.Persistence.repositories.general_format_tests;

internal class GeneralTestAnswersRepository : IGeneralTestAnswersRepository
{
    private readonly TestCreationDbContext _db;
    public GeneralTestAnswersRepository(TestCreationDbContext db) { _db = db; }

    public async Task<GeneralTestAnswer?> GetById(GeneralTestAnswerId id) =>
        await _db.GeneralTestAnswers.FindAsync(id);

    public async Task Update(GeneralTestAnswer question) {
        _db.GeneralTestAnswers.Update(question);
        await _db.SaveChangesAsync();
    }
}
