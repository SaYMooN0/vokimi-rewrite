using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.domain.entity;
using TestManagingService.Application.Common.interfaces.repositories.feedback_records;
using TestManagingService.Domain.FeedbackRecordAggregate;
using TestManagingService.Domain.FeedbackRecordAggregate.filters;
using TestManagingService.Domain.FeedbackRecordAggregate.general_test;
using TestManagingService.Domain.FeedbackRecordAggregate.sort_options;

namespace TestManagingService.Infrastructure.Persistence.repositories.feedback_records;

public class GeneralTestFeedbackRecordsRepository : IGeneralTestFeedbackRecordsRepository
{
    private readonly TestManagingDbContext _db;

    public GeneralTestFeedbackRecordsRepository(TestManagingDbContext db) {
        _db = db;
    }

    public async Task Add(GeneralTestFeedbackRecord feedbackRecord) {
        await _db.GeneralTestFeedbackRecords.AddAsync(feedbackRecord);
        await _db.SaveChangesAsync();
    }

    public Task<GeneralTestFeedbackRecord[]> ListForTestAsNoTracking(TestId testId) =>
        _db.GeneralTestFeedbackRecords
            .AsNoTracking()
            .Where(f => f.TestId == testId)
            .ToArrayAsync();

    public Task<GeneralTestFeedbackRecord[]> ListFilteredForTestAsNoTracking(
        TestId testId, GeneralTestFeedbackRecordsQueryFilter filter
    ) => _db.GeneralTestFeedbackRecords
        .AsNoTracking()
        .Where(f => f.TestId == testId)
        .WithAllFiltersAndSorting(filter)
        .ToArrayAsync();
}

file static class GeneralTestFeedbackRecordsQueryExtensions
{
    public static IQueryable<GeneralTestFeedbackRecord> WithAllFiltersAndSorting(
        this IQueryable<GeneralTestFeedbackRecord> query, GeneralTestFeedbackRecordsQueryFilter filter
    ) => query
        .WithDateFilter(filter)
        .WithVisibilityFilter(filter)
        .WithTextLengthFilter(filter.TextLengthFrom, filter.TextLengthTo)
        .WithSorting(filter.SortOption);


    private static IQueryable<GeneralTestFeedbackRecord> WithTextLengthFilter(
        this IQueryable<GeneralTestFeedbackRecord> query, ushort? textLengthFrom, ushort? textLengthTo
    ) {
        if (textLengthFrom.HasValue)
            query = query.Where(f => f.Text.Length >= textLengthFrom.Value);
        if (textLengthTo.HasValue)
            query = query.Where(f => f.Text.Length <= textLengthTo.Value);

        return query;
    }

    private static IQueryable<GeneralTestFeedbackRecord> WithSorting(
        this IQueryable<GeneralTestFeedbackRecord> query, GeneralTestFeedbackRecordsSortOption sorting
    ) => sorting switch {
        GeneralTestFeedbackRecordsSortOption.Randomized => query,
        GeneralTestFeedbackRecordsSortOption.Newest => query.OrderByDescending(f => f.CreatedOn),
        GeneralTestFeedbackRecordsSortOption.Oldest => query.OrderBy(f => f.CreatedOn),
        GeneralTestFeedbackRecordsSortOption.Shortest => query.OrderBy(f => f.Text.Length),
        GeneralTestFeedbackRecordsSortOption.Longest => query.OrderByDescending(f => f.Text.Length),
        _ => throw new ArgumentOutOfRangeException(nameof(sorting), sorting, "Unsupported sort option.")
    };
}