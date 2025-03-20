using TestManagingService.Domain.FeedbackRecordAggregate.test_formats_shared;

namespace TestManagingService.Domain.FeedbackRecordAggregate.tier_list_test;

public record class TierListTestFeedbackRecordsFilter(
    DateTime? CreatedDateFrom,
    DateTime? CreatedDateTo,
    ushort? TextLengthFrom,
    ushort? TextLengthTo,
    bool ShowAnonymous,
    bool ShowNonAnonymous,
    TestFeedbackRecordsSortOption SortOption
) : TestFormatsSharedFeedbackRecordsFilter
(
    CreatedDateFrom: CreatedDateFrom,
    CreatedDateTo: CreatedDateTo,
    SortOption
);