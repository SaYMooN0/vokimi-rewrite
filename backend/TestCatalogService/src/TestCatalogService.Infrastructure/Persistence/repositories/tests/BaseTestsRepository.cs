using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client.Events;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate;
using TestCatalogService.Domain.TestAggregate.formats_shared;
using TestCatalogService.Domain.TestAggregate.formats_shared.ratings;

namespace TestCatalogService.Infrastructure.Persistence.repositories.tests;

internal class BaseTestsRepository : IBaseTestsRepository
{
    private readonly TestCatalogDbContext _db;

    public BaseTestsRepository(TestCatalogDbContext db) {
        _db = db;
    }

    public async Task<BaseTest?> GetById(TestId testId) =>
        await _db.BaseTests.FindAsync(testId);

    public async Task<ErrOr<AppUserId>> GetTestCreatorId(TestId testId) {
        BaseTest? test = await _db.BaseTests.FindAsync(testId);
        if (test is not null) {
            return test.CreatorId;
        }

        return Err.ErrPresets.TestNotFound(testId);
    }

    public async Task Update(BaseTest test) {
        _db.BaseTests.Update(test);
        await _db.SaveChangesAsync();
    }

    public async Task<BaseTest?> GetWithRatingsAsNoTracking(TestId testId) =>
        await _db.BaseTests
            .Include(t => EF.Property<ICollection<TestRating>>(t, "_ratings"))
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == testId);


    public async Task<BaseTest?> GetWithRatings(TestId testId) =>
        await _db.BaseTests
            .Include(t => EF.Property<ICollection<TestRating>>(t, "_ratings"))
            .FirstOrDefaultAsync(t => t.Id == testId);
}