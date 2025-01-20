using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Infrastructure.Persistence.repositories;

internal class GeneralFormatTestsRepository : IGeneralFormatTestsRepository
{
    private readonly TestCreationDbContext _db;

    public GeneralFormatTestsRepository(TestCreationDbContext db) { _db = db; }
    public async Task AddNew(GeneralFormatTest test) {
        await _db.GeneralFormatTests.AddAsync(test);
        await _db.SaveChangesAsync();

    }

    public async Task<GeneralFormatTest?> GetById(TestId testId) {
        return await _db.GeneralFormatTests.FindAsync(testId);
    }

    public Task<GeneralFormatTest?> GetWithQuestions(TestId testId) {
        return _db.GeneralFormatTests
            .Include(t => EF.Property<List<GeneralTestQuestion>>(t, "_questions"))
            .FirstOrDefaultAsync(t => t.Id == testId);
    }

    public async Task Update(GeneralFormatTest test) {
        _db.GeneralFormatTests.Update(test);
        await _db.SaveChangesAsync();
    }
}
