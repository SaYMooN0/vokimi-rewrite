using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestTakingService.Application.Common.interfaces.repositories;
using TestTakingService.Domain.AppUserAggregate;

namespace TestTakingService.Infrastructure.Persistence.repositories;

internal class AppUsersRepository: IAppUsersRepository
{
    private TestTakingDbContext _db;
    public AppUsersRepository(TestTakingDbContext db) {
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