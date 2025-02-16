using System.Collections.Immutable;
using TestCatalogService.Application.Common.interfaces.repositories;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Infrastructure.Persistence.repositories;

internal class TestCommentsRepository : ITestCommentsRepository
{
    private TestCatalogDbContext _db;

    public TestCommentsRepository(TestCatalogDbContext db) {
        _db = db;
    }

    private const int packageSize = 50;

    public ImmutableArray<TestComment> GetUnhiddenCommentsPackage(int packageNumber) =>

    public ImmutableArray<TestComment> GetUnhiddenFilteredCommentsPackage(int packageNumber) =>
}