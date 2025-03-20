namespace TestManagingService.Domain.FeedbackRecordAggregate.test_formats_shared;

public abstract record class TestFormatsSharedFeedbackRecordsFilter(
    DateTime? CreatedDateFrom,
    DateTime? CreatedDateTo,
    TestFeedbackRecordsSortOption SortOption
);