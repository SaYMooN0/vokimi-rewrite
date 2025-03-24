using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.domain.entity;
using TestManagingService.Application.Common.interfaces.repositories.feedback_records;
using TestManagingService.Domain.FeedbackRecordAggregate.filters;
using TestManagingService.Domain.FeedbackRecordAggregate.sort_options;
using TestManagingService.Domain.FeedbackRecordAggregate.tier_list_test;

namespace TestManagingService.Infrastructure.Persistence.repositories.feedback_records;

internal class TierListTestFeedbackRecordsRepository : ITierListTestFeedbackRecordsRepository
{
    private readonly TestManagingDbContext _db;

    public TierListTestFeedbackRecordsRepository(TestManagingDbContext db) {
        _db = db;
    }

    public async Task Add(TierListTestFeedbackRecord feedbackRecord) {
        await _db.TierListTestFeedbackRecords.AddAsync(feedbackRecord);
        await _db.SaveChangesAsync();
    }

   public Task<TierListTestFeedbackRecord[]> ListForTestAsNoTracking(TestId testId) =>
        _db.TierListTestFeedbackRecords
            .AsNoTracking()
            .Where(f => f.TestId == testId)
            .ToArrayAsync();

    public Task<TierListTestFeedbackRecord[]> ListFilteredForTestAsNoTracking(
        TestId testId, TierListTestFeedbackRecordsQueryFilter filter
    ) => _db.TierListTestFeedbackRecords
        .AsNoTracking()
        .Where(f => f.TestId == testId)
        .WithAllFiltersAndSorting(filter)
        .ToArrayAsync();
}

file static class TierListTestFeedbackRecordsQueryExtensions
{
    public static IQueryable<TierListTestFeedbackRecord> WithAllFiltersAndSorting(
        this IQueryable<TierListTestFeedbackRecord> query, TierListTestFeedbackRecordsQueryFilter filter
    ) => query
        .WithDateFilter(filter)
        .WithVisibilityFilter(filter)
        .WithTextLengthFilter(filter.TextLengthFrom, filter.TextLengthTo)
        .WithSorting(filter.SortOption);


    private static IQueryable<TierListTestFeedbackRecord> WithTextLengthFilter(
        this IQueryable<TierListTestFeedbackRecord> query, ushort? textLengthFrom, ushort? textLengthTo
    ) {
        if (textLengthFrom.HasValue)
            query = query.Where(f => f.Text.Length >= textLengthFrom.Value);
        if (textLengthTo.HasValue)
            query = query.Where(f => f.Text.Length <= textLengthTo.Value);

        return query;
    }

    private static IQueryable<TierListTestFeedbackRecord> WithSorting(
        this IQueryable<TierListTestFeedbackRecord> query, TierListTestFeedbackRecordsSortOption sorting
    ) => sorting switch {
        TierListTestFeedbackRecordsSortOption.Randomized => query,
        TierListTestFeedbackRecordsSortOption.Newest => query.OrderByDescending(f => f.CreatedOn),
        TierListTestFeedbackRecordsSortOption.Oldest => query.OrderBy(f => f.CreatedOn),
        TierListTestFeedbackRecordsSortOption.Shortest => query.OrderBy(f => f.Text.Length),
        TierListTestFeedbackRecordsSortOption.Longest => query.OrderByDescending(f => f.Text.Length),
        _ => throw new ArgumentOutOfRangeException(nameof(sorting), sorting, "Unsupported sort option.")
    };
}