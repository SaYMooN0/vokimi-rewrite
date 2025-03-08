using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.domain.entity;
using TestManagingService.Application.Common.interfaces.repositories.feedback_records;
using TestManagingService.Domain.TestFeedbackRecordAggregate.general_test;

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

    public Task<GeneralTestFeedbackRecord[]> ListForTestAsNoTracking(TestId testId) => _db.GeneralTestFeedbackRecords
        .AsNoTracking()
        .Where(f => f.TestId == testId)
        .ToArrayAsync();

    public Task<GeneralTestFeedbackRecord[]> ListFilteredForTestAsNoTracking(
        TestId testId, GeneralTestFeedbackRecordsFilter filter
    ) => _db.GeneralTestFeedbackRecords
        .AsNoTracking()
        .Where(f => f.TestId == testId)
        .WithFilterAndSorting(filter)
        .ToArrayAsync();
}

file static class GeneralTestFeedbackRecordsQueryExtensions
{
    public static IQueryable<GeneralTestFeedbackRecord> WithFilterAndSorting(
        this IQueryable<GeneralTestFeedbackRecord> query, GeneralTestFeedbackRecordsFilter filter
    ) => query
        .WithDateFilter(filter.CreatedDateFrom, filter.CreatedDateTo)
        .WithTextLengthFilter(filter.TextLengthFrom, filter.TextLengthTo)
        .WithVisibilityFilter(filter.ShowAnonymous, filter.ShowNonAnonymous)
        .WithSorting(filter.SortOption);

    private static IQueryable<GeneralTestFeedbackRecord> WithDateFilter(
        this IQueryable<GeneralTestFeedbackRecord> query, DateTime? dateFrom, DateTime? dateTo
    ) {
        if (dateFrom.HasValue)
            query = query.Where(f => f.CreatedOn >= dateFrom.Value);
        if (dateTo.HasValue)
            query = query.Where(f => f.CreatedOn <= dateTo.Value);

        return query;
    }

    private static IQueryable<GeneralTestFeedbackRecord> WithTextLengthFilter(
        this IQueryable<GeneralTestFeedbackRecord> query, ushort? textLengthFrom, ushort? textLengthTo
    ) {
        if (textLengthFrom.HasValue)
            query = query.Where(f => f.Text.Length >= textLengthFrom.Value);
        if (textLengthTo.HasValue)
            query = query.Where(f => f.Text.Length <= textLengthTo.Value);

        return query;
    }

    private static IQueryable<GeneralTestFeedbackRecord> WithVisibilityFilter(
        this IQueryable<GeneralTestFeedbackRecord> query, bool showAnonymous, bool showNonAnonymous
    ) {
        if (!showAnonymous)
            query = query.Where(f => !f.WasLeftAnonymously);
        if (!showNonAnonymous)
            query = query.Where(f => f.WasLeftAnonymously);

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