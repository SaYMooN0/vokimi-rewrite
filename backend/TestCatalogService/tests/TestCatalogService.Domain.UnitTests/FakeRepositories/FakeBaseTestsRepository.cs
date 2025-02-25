using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate;

namespace TestCatalogService.Domain.UnitTests.FakeRepositories;

public class FakeBaseTestsRepository : IBaseTestsRepository
{
    public Func<TestId, Task<ErrOr<AppUserId>>> GetTestCreatorIdFunc { get; init; } =
        (testId) => throw new Exception("Implementation not provided");

    public Func<TestId, Task<BaseTest?>> GetByIdFunc { get; init; } =
        (testId) => throw new Exception("Implementation not provided");


    public Func<TestId, Task<BaseTest?>> GetWithRatingsAsNoTrackingFunc { get; init; } =
        (testId) => throw new Exception("Implementation not provided");


    public Func<TestId, Task<BaseTest?>> GetWithRatingsFunc { get; init; } =
        (testId) => throw new Exception("Implementation not provided");


    public Func<BaseTest, Task> UpdateFunc { get; init; } =
        (testId) => throw new Exception("Implementation not provided");

    public Task<BaseTest?> GetById(TestId testId) => GetByIdFunc(testId);

    public Task<ErrOr<AppUserId>> GetTestCreatorId(TestId testId) => GetTestCreatorIdFunc(testId);

    public Task Update(BaseTest test) => UpdateFunc(test);

    public Task<BaseTest?> GetWithRatingsAsNoTracking(TestId testId) => GetWithRatingsAsNoTrackingFunc(testId);

    public Task<BaseTest?> GetWithRatings(TestId testId) => GetWithRatingsFunc(testId);
}