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
    public async Task<ErrListOrNothing> AddTestEditorsToTest(TestId testId, HashSet<AppUserId> newEditors) {
        ErrList errs = new();
        BaseTest? t = await _db.BaseTests.FindAsync(testId);
        if (t is null) { return TestNotFoungErr(); }
        foreach (var editorId in newEditors) {
            if (t.IsUserCreator(editorId)) {
                errs.Add(new("Test creator cannot be added as an editor"));
                continue;
            }
            AppUser? editor = await _db.AppUsers.FindAsync(editorId);
            if (editor is null) {
                errs.Add(Err.ErrFactory.NotFound($"User {editorId} not found", details: $"User {editorId} not found"));
                continue;
            }
            var addingResult = t.AddEditor(editor.Id);
            errs.AddPossibleErr(addingResult);
        }
        await _db.SaveChangesAsync();
        return ErrListOrNothing.Nothing;
    }
}
