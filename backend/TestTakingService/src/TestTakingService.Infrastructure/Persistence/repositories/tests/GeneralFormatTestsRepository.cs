using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.domain;
using TestTakingService.Application.Common.interfaces.repositories.tests;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Infrastructure.Persistence.repositories.tests;

public class GeneralFormatTestsRepository : IGeneralFormatTestsRepository
{
    private readonly TestTakingDbContext _db;

    public GeneralFormatTestsRepository(TestTakingDbContext db) {
        _db = db;
    }

    public async Task Add(GeneralFormatTest test) {
        await _db.GeneralFormatTests.AddAsync(test);
        await _db.SaveChangesAsync();
    }

    public async Task<GeneralFormatTest?> GetWithQuestionWithAnswers(TestId testId) => await _db.GeneralFormatTests
        .Include(t => EF.Property<ImmutableArray<GeneralTestQuestion>>(t, "_questions"))
            .ThenInclude(q => EF.Property<ImmutableArray<GeneralTestQuestion>>(q, "_answers"))
        .FirstOrDefaultAsync(t => t.Id == testId);
}