using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.formats_shared.test_styles;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.formats_shared;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Infrastructure.Persistence.repositories;

internal class GeneralFormatTestsRepository : IGeneralFormatTestsRepository
{
    private readonly TestCreationDbContext _db;

    public GeneralFormatTestsRepository(TestCreationDbContext db) {
        _db = db;
    }

    public async Task AddNew(GeneralFormatTest test) {
        await _db.GeneralFormatTests.AddAsync(test);
        await _db.SaveChangesAsync();
    }

    public async Task Delete(TestId testId) {
        await _db.GeneralFormatTests
            .Where(t => t.Id == testId)
            .ExecuteDeleteAsync();
    }

    public async Task<GeneralFormatTest?> GetById(TestId testId) {
        return await _db.GeneralFormatTests.FindAsync(testId);
    }

    public async Task<GeneralFormatTest?> GetWithEverything(TestId testId) =>
        await _db.GeneralFormatTests
            .Include(t => EF.Property<TestStylesSheet>(t, "_styles"))
            .Include(t => EF.Property<TestTagsList>(t, "_tags"))
            .Include(t => EF.Property<List<GeneralTestResult>>(t, "_results"))
            .FirstOrDefaultAsync(t => t.Id == testId);

    public async Task<GeneralFormatTest?> GetWithResults(TestId testId) {
        return await _db.GeneralFormatTests
            .Include(t => EF.Property<List<GeneralTestResult>>(t, "_results"))
            .FirstOrDefaultAsync(t => t.Id == testId);
    }

    public async Task Update(GeneralFormatTest test) {
        _db.GeneralFormatTests.Update(test);
        await _db.SaveChangesAsync();
    }
}