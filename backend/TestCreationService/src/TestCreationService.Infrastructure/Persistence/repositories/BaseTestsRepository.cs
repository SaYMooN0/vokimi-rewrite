using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.formats_shared.test_styles;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate;
using TestCreationService.Domain.TestAggregate.formats_shared;

namespace TestCreationService.Infrastructure.Persistence.repositories;

internal class BaseTestsRepository : IBaseTestsRepository
{
    private readonly TestCreationDbContext _db;

    public BaseTestsRepository(TestCreationDbContext db) { _db = db; }
    public async Task<ErrOr<bool>> CheckIfUserIsTestCreator(TestId testId, AppUserId userId) {
        BaseTest? t = await _db.BaseTests.FindAsync(testId);
        if (t is null) { return Err.ErrPresets.TestNotFound(testId); }
        return t.IsUserCreator(userId);
    }

    public async Task<ErrOr<HashSet<AppUserId>>> GetUserIdsWithPermissionToEditTest(TestId testId) {
        BaseTest? t = await _db.BaseTests.FindAsync(testId);
        if (t is null) { return Err.ErrPresets.TestNotFound(testId); }
        return t.TestEditorsWithCreator();
    }

    public async Task<BaseTest?> GetById(TestId testId) {
        return await _db.BaseTests.FindAsync(testId);
    }
    public async Task Update(BaseTest test) {
        _db.BaseTests.Update(test);
        await _db.SaveChangesAsync();
    }

    public async Task<BaseTest?> GetWithStyles(TestId testId) =>
        await _db.BaseTests
            .Include(t => EF.Property<TestStylesSheet>(t, "_styles"))
            .FirstOrDefaultAsync(t => t.Id == testId);

    public async Task<BaseTest?> GetWithTags(TestId testId) =>
        await _db.BaseTests
            .Include(t => EF.Property<TestTagsList>(t, "_tags"))
            .FirstOrDefaultAsync(t => t.Id == testId);
}
