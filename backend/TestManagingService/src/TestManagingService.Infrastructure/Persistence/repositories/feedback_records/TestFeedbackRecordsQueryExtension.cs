using TestManagingService.Domain.FeedbackRecordAggregate;
using TestManagingService.Domain.FeedbackRecordAggregate.filters;

namespace TestManagingService.Infrastructure.Persistence.repositories.feedback_records;

internal static class TestFeedbackRecordsQueryExtension
{
    public static IQueryable<T> WithDateFilter<T>(
        this IQueryable<T> query, BaseTestFeedbackRecordsQueryFilter filter
    ) where T : BaseTestFeedbackRecord {
        if (filter.CreatedDateFrom.HasValue)
            query = query.Where(f => f.CreatedOn >= filter.CreatedDateFrom.Value);
        if (filter.CreatedDateTo.HasValue)
            query = query.Where(f => f.CreatedOn <= filter.CreatedDateTo.Value);
        return query;
    }

    public static IQueryable<T> WithVisibilityFilter<T>(
        this IQueryable<T> query, BaseTestFeedbackRecordsQueryFilter filter
    ) where T : BaseTestFeedbackRecord {
        if (!filter.ShowAnonymous)
            query = query.Where(f => !f.WasLeftAnonymously);
        if (!filter.ShowNonAnonymous)
            query = query.Where(f => f.WasLeftAnonymously);

        return query;
    }
}