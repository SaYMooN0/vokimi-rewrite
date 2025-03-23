namespace TestManagingService.Domain.FeedbackRecordAggregate.filters;

public abstract record class BaseTestFeedbackRecordsQueryFilter(
    DateTime? CreatedDateFrom,
    DateTime? CreatedDateTo,
    bool ShowAnonymous,
    bool ShowNonAnonymous
) { }