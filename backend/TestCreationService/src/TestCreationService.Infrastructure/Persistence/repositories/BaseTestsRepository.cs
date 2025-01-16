using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.AppUserAggregate;
using TestCreationService.Domain.TestAggregate;

namespace TestCreationService.Infrastructure.Persistence.repositories;

internal class BaseTestsRepository : IBaseTestsRepository
{
    private readonly TestCreationDbContext _db;

    public BaseTestsRepository(TestCreationDbContext db) { _db = db; }
    private Err TestNotFoungErr(string message = "Unknown test") => Err.ErrFactory.NotFound(message);
    public async Task<ErrOr<bool>> CheckIfUserIsTestCreator(TestId testId, AppUserId userId) {
        BaseTest? t = await _db.BaseTests.FindAsync(testId);
        if (t is null) { return TestNotFoungErr(); }
        return t.IsUserCreator(userId);
    }

    public async Task<ErrOr<HashSet<AppUserId>>> GetUserIdsWithPermissionToEditTest(TestId testId) {
        BaseTest? t = await _db.BaseTests.FindAsync(testId);
        if (t is null) { return TestNotFoungErr(); }
        return t.TestEditorsWithCreator();
    }

    public async Task<BaseTest?> GetById(TestId testId) {
        return await _db.BaseTests.FindAsync(testId);
    }
    public async Task Update(BaseTest test) {
        _db.BaseTests.Update(test);
        await _db.SaveChangesAsync();
    }
}
