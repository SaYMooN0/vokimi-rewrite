using SharedKernel.Common.domain.entity;
using TestManagingService.Application.Common.interfaces.repositories;
using TestManagingService.Domain.AppUserAggregate;

namespace TestManagingService.Infrastructure.Persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private TestManagingDbContext _db;
    public AppUsersRepository(TestManagingDbContext db) {
        _db = db;
    }

    public async Task<AppUser?> GetById(AppUserId appUserId) =>
        await _db.AppUsers.FindAsync(appUserId);

    public async Task Add(AppUser appUser) {
        _db.AppUsers.Add(appUser);
        await _db.SaveChangesAsync();
    }

    public async Task Update(AppUser appUser) {
        _db.AppUsers.Update(appUser);
        await _db.SaveChangesAsync();
    }
}