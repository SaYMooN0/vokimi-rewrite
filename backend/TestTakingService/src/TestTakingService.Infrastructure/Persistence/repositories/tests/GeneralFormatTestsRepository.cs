using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using TestTakingService.Application.Common.interfaces.repositories.tests;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Infrastructure.Persistence.repositories.tests;

internal class GeneralFormatTestsRepository : IGeneralFormatTestsRepository
{
    private readonly TestTakingDbContext _db;

    public GeneralFormatTestsRepository(TestTakingDbContext db) {
        _db = db;
    }

    public async Task Add(GeneralFormatTest test) {
        await _db.GeneralFormatTests.AddAsync(test);
        await _db.SaveChangesAsync();
    }

    public async Task<GeneralFormatTest?> GetWithQuestionWithAnswers(TestId testId) =>
        await _db.GeneralFormatTests
            .Include(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(t => t.Id == testId);

    public async Task<GeneralFormatTest?> GetWithQuestionsAnswersAndResults(TestId testId) =>
        await _db.GeneralFormatTests
            .Include(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .Include(t => t.Results)
            .FirstOrDefaultAsync(t => t.Id == testId);

    public async Task<GeneralFormatTest?> GetWithResults(TestId testId) => await _db.GeneralFormatTests
        .Include(t => t.Results)
        .FirstOrDefaultAsync(t => t.Id == testId);
}