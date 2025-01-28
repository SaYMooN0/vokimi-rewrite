using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.test_styles;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.AppUserAggregate;
using TestCreationService.Domain.TestAggregate;

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
}
