using System.Collections.Immutable;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.Common.filters;
using TestCatalogService.Domain.Common.interfaces.repositories;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Domain.UnitTests.FakeRepositories;

public class FakeTestCommentsRepository : ITestCommentsRepository
{
    public Func<TestComment, Task> AddFunc { get; init; } =
        (comment) => throw new Exception("Implementation not provided");

    public Func<TestCommentId, Task<TestComment?>> GetByIdFunc { get; init; } =
        (commentId) => throw new Exception("Implementation not provided");

    public Func<TestComment, Task> UpdateFunc { get; init; } =
        (comment) => throw new Exception("Implementation not provided");

    public Func<TestId, uint, AppUserId?, Task<ImmutableArray<TestCommentWithViewerVote>>>
        GetCommentsPackageForViewerFunc { get; init; } =
        (testId, packageNumber, viewer) => throw new Exception("Implementation not provided");

    public Func<TestId, uint, AppUserId?, ListTestCommentsFilter, Task<ImmutableArray<TestCommentWithViewerVote>>>
        GetFilteredCommentsPackageForViewerFunc { get; init; } =
        (testId, packageNumber, viewer, filter) => throw new Exception("Implementation not provided");

    public Func<TestCommentId, uint, AppUserId?, ListTestCommentsFilter,
        Task<ImmutableArray<TestCommentWithViewerVote>>> GetFilteredAnswersPackageForViewerFunc { get; init; } =
        (parentCommentId, packageNumber, viewer, filter) => throw new Exception("Implementation not provided");

    public Func<TestCommentId, uint, AppUserId?, Task<ImmutableArray<TestCommentWithViewerVote>>>
        GetAnswersPackageForViewerFunc { get; init; } =
        (parentCommentId, packageNumber, viewer) => throw new Exception("Implementation not provided");

    public Task Add(TestComment comment) => AddFunc(comment);

    public Task<TestComment?> GetById(TestCommentId commentId) => GetByIdFunc(commentId);

    public Task Update(TestComment comment) => UpdateFunc(comment);

    public Task<ImmutableArray<TestCommentWithViewerVote>> GetCommentsPackageForViewer(
        TestId testId, uint packageNumber, AppUserId? viewer
    ) => GetCommentsPackageForViewerFunc(testId, packageNumber, viewer);

    public Task<ImmutableArray<TestCommentWithViewerVote>> GetFilteredCommentsPackageForViewer(
        TestId testId, uint packageNumber, AppUserId? viewer, ListTestCommentsFilter filter
    ) => GetFilteredCommentsPackageForViewerFunc(testId, packageNumber, viewer, filter);

    public Task<ImmutableArray<TestCommentWithViewerVote>> GetFilteredAnswersPackageForViewer(
        TestCommentId parentCommentId, uint packageNumber, AppUserId? viewer, ListTestCommentsFilter filter
    ) => GetFilteredAnswersPackageForViewerFunc(parentCommentId, packageNumber, viewer, filter);

    public Task<ImmutableArray<TestCommentWithViewerVote>> GetAnswersPackageForViewer(
        TestCommentId parentCommentId, uint packageNumber, AppUserId? viewer
    ) => GetAnswersPackageForViewerFunc(parentCommentId, packageNumber, viewer);
}