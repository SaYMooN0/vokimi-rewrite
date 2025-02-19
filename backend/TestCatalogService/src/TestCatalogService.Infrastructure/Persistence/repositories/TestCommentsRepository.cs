using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.Common.filters;
using TestCatalogService.Domain.Common.interfaces.repositories;
using TestCatalogService.Domain.Common.sort_options;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Infrastructure.Persistence.repositories;

internal class TestCommentsRepository : ITestCommentsRepository
{
    private TestCatalogDbContext _db;

    public TestCommentsRepository(TestCatalogDbContext db) {
        _db = db;
    }

    private const int PackageSize = 50;

    public async Task Add(TestComment comment) {
        await _db.TestComments.AddAsync(comment);
        await _db.SaveChangesAsync();
    }

    public async Task<TestComment?> GetById(TestCommentId commentId) =>
        await _db.TestComments.FindAsync(commentId);

    public async Task Update(TestComment comment) {
        _db.TestComments.Update(comment);
        await _db.SaveChangesAsync();
    }

    public async Task<ImmutableArray<TestCommentWithViewerVote>> GetCommentsPackageForViewer(
        TestId testId, uint packageNumber, AppUserId? viewer
    ) => (await _db.TestComments
            .Where(c => c.TestId == testId)
            .Skip((int)packageNumber * PackageSize)
            .Take(PackageSize)
            .Select(c => new { Comment = c, Vote = c.Votes.FirstOrDefault(v => v.UserId == viewer) })
            .ToArrayAsync()
        )
        .Select(c => TestCommentWithViewerVote.Create(c.Comment, c.Vote))
        .ToImmutableArray();

    public async Task<ImmutableArray<TestCommentWithViewerVote>> GetFilteredCommentsPackageForViewer(TestId testId,
        uint packageNumber, AppUserId? viewer,
        ListTestCommentsFilter filter
    ) => (await _db.TestComments
            .Where(c => c.TestId == testId)
            .WithFilterAndSorting(filter)
            .Skip((int)packageNumber * PackageSize)
            .Take(PackageSize)
            .Select(c => new { Comment = c, Vote = c.Votes.FirstOrDefault(v => v.UserId == viewer) })
            .ToArrayAsync()
        )
        .Select(c => TestCommentWithViewerVote.Create(c.Comment, c.Vote))
        .ToImmutableArray();
}

file static class TestCommentsQueryExtensions
{
    public static IQueryable<TestComment> WithFilterAndSorting(
        this IQueryable<TestComment> query, ListTestCommentsFilter filter
    ) => query
        .WithAnswersCountFilter(filter.MinAnswersCount, filter.MaxAnswersCount)
        .WithVotesRatingFilter(filter.MinVotesRating, filter.MaxVotesRating)
        .WithVotesCountFilter(filter.MinVotesCount, filter.MaxVotesCount)
        .WithDateFilter(filter.DateFrom, filter.DateTo)
        .WithVisibilityFilter(filter.ShowHidden, filter.ShowDeleted)
        .WithAttachmentFilter(filter.WithAttachments)
        .WithSorting(filter.Sorting);


    private static IQueryable<TestComment> WithAnswersCountFilter(
        this IQueryable<TestComment> query, uint? minAnswersCount, uint? maxAnswersCount
    ) {
        if (minAnswersCount.HasValue)
            query = query.Where(c => c.CurrentAnswersCount >= minAnswersCount.Value);
        if (maxAnswersCount.HasValue)
            query = query.Where(c => c.CurrentAnswersCount <= maxAnswersCount.Value);

        return query;
    }

    private static IQueryable<TestComment> WithVotesRatingFilter(
        this IQueryable<TestComment> query, int? minVotesRating, int? maxVotesRating
    ) {
        if (minVotesRating.HasValue)
            query = query.Where(c => (c.UpVotesCount - c.DownVotesCount) >= minVotesRating.Value);
        if (maxVotesRating.HasValue)
            query = query.Where(c => (c.UpVotesCount - c.DownVotesCount) <= maxVotesRating.Value);

        return query;
    }

    private static IQueryable<TestComment> WithVotesCountFilter(
        this IQueryable<TestComment> query, uint? minVotesCount, uint? maxVotesCount
    ) {
        if (minVotesCount.HasValue)
            query = query.Where(c => (c.UpVotesCount + c.DownVotesCount) >= minVotesCount.Value);
        if (maxVotesCount.HasValue)
            query = query.Where(c => (c.UpVotesCount + c.DownVotesCount) <= maxVotesCount.Value);

        return query;
    }

    private static IQueryable<TestComment> WithDateFilter(
        this IQueryable<TestComment> query, DateTime? dateFrom, DateTime? dateTo
    ) {
        if (dateFrom.HasValue)
            query = query.Where(c => c.CreatedAt >= dateFrom.Value);
        if (dateTo.HasValue)
            query = query.Where(c => c.CreatedAt <= dateTo.Value);

        return query;
    }

    private static IQueryable<TestComment> WithVisibilityFilter(
        this IQueryable<TestComment> query, bool showHidden, bool showDeleted
    ) {
        if (!showHidden)
            query = query.Where(c => !c.IsHidden);
        if (!showDeleted)
            query = query.Where(c => !c.IsDeleted);

        return query;
    }

    private static IQueryable<TestComment> WithAttachmentFilter(
        this IQueryable<TestComment> query, FilterTriState withAttachments
    ) {
        if (withAttachments == FilterTriState.Yes)
            query = query.Where(c => c.HasAttachment);
        else if (withAttachments == FilterTriState.No)
            query = query.Where(c => !c.HasAttachment);

        return query;
    }

    private static IQueryable<TestComment> WithSorting(
        this IQueryable<TestComment> query, TestCommentsSortOption sorting
    ) => sorting switch {
        TestCommentsSortOption.Randomized => query,
        TestCommentsSortOption.Newest => query.OrderByDescending(c => c.CreatedAt),
        TestCommentsSortOption.Oldest => query.OrderBy(c => c.CreatedAt),
        TestCommentsSortOption.HighestRating => query.OrderByDescending(c => (c.UpVotesCount - c.DownVotesCount)),
        TestCommentsSortOption.LowestRating => query.OrderBy(c => (c.UpVotesCount - c.DownVotesCount)),
        TestCommentsSortOption.MaximumAnswers => query.OrderByDescending(c => c.CurrentAnswersCount),
        _ => throw new ArgumentOutOfRangeException(nameof(sorting), sorting, "Unsupported sort option.")
    };
}