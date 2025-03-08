using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.AppUserAggregate;
using TestCatalogService.Domain.Common.interfaces.repositories;

namespace TestCatalogService.Domain.UnitTests.FakeRepositories;

public class FakeAppUsersRepository : IAppUsersRepository
{
    public Func<AppUserId, Task<AppUser?>> GetByIdFunc { get; init; } =
        (appUserId) => throw new Exception("Implementation not provided");

    public Func<AppUser, Task> AddFunc { get; init; } =
        (appUser) => throw new Exception("Implementation not provided");

    public Func<AppUser, Task> UpdateFunc { get; init; } =
        (appUser) => throw new Exception("Implementation not provided");

    public Func<AppUserId, Task<bool>> DoesUserExistFunc { get; init; } =
        (appUserId) => throw new Exception("Implementation not provided");

    public Task<AppUser?> GetById(AppUserId appUserId) => GetByIdFunc(appUserId);

    public Task Add(AppUser appUser) => AddFunc(appUser);

    public Task Update(AppUser appUser) => UpdateFunc(appUser);

    public Task<bool> DoesUserExist(AppUserId appUserId) => DoesUserExistFunc(appUserId);
}
