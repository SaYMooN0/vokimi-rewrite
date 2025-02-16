using System.Collections.Immutable;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Application.Common.interfaces.repositories;

public interface ITestCommentsRepository
{
    public ImmutableArray<TestComment> GetUnhiddenCommentsPackage(int packageNumber);
    public ImmutableArray<TestComment> GetUnhiddenFilteredCommentsPackage(int packageNumber);
}