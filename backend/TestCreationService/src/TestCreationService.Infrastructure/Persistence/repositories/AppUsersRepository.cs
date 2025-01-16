using SharedKernel.Common.EntityIds;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.AppUserAggregate;

namespace TestCreationService.Infrastructure.Persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly TestCreationDbContext _db;

    public AppUsersRepository(TestCreationDbContext db) { _db = db; }

    public async Task Add(AppUser appUser) {
        _db.AppUsers.Add(appUser);
        await _db.SaveChangesAsync();
    }

    public async Task<AppUser?> GetById(AppUserId id) {
        return await _db.AppUsers.FindAsync(id);
    }

    public async Task Update(AppUser appUser) {
        _db.Update(appUser);
        await _db.SaveChangesAsync();
    }
    public async Task UpdateRange(IEnumerable<AppUser> appUsers) {
        _db.UpdateRange(appUsers);
        await _db.SaveChangesAsync();
    }
}
