using System.Collections.Immutable;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Application.Common.filters;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Application.Common.interfaces.repositories;

public record TestCommentWithViewerVote(TestComment Comment, bool? UserVote);

public interface ITestCommentsRepository
{
    public Task Add(TestComment comment);
    public Task<TestComment?> GetById(TestCommentId commentId);

    public Task<ImmutableArray<TestCommentWithViewerVote>> GetCommentsPackageForViewer(
        TestId testId,
        uint packageNumber,
        AppUserId? viewer
    );

    public Task<ImmutableArray<TestCommentWithViewerVote>> GetFilteredCommentsPackageForViewer(
        TestId testId,
        uint packageNumber,
        AppUserId? viewer,
        ListTestCommentsFilter filter
    );
}