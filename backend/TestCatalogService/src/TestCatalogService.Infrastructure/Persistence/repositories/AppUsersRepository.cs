using SharedKernel.Common.domain;
using TestCatalogService.Application.Common.interfaces.repositories;
using TestCatalogService.Domain.AppUserAggregate;

namespace TestCatalogService.Infrastructure.Persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private TestCatalogDbContext _db;
    public AppUsersRepository(TestCatalogDbContext db) {
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