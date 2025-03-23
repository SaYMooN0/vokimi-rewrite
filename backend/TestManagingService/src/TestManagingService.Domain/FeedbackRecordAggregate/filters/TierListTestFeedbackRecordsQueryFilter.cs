namespace TestManagingService.Domain.FeedbackRecordAggregate.filters;

public record class TierListTestFeedbackRecordsQueryFilter(
    DateTime? CreatedDateFrom,
    DateTime? CreatedDateTo,
    ushort? TextLengthFrom,
    ushort? TextLengthTo,
    bool ShowAnonymous,
    bool ShowNonAnonymous,
    TierListTestFeedbackRecordsSortOption SortOption
) : BaseTestFeedbackRecordsQueryFilter
(
    CreatedDateFrom: CreatedDateFrom,
    CreatedDateTo: CreatedDateTo,
    ShowAnonymous: ShowAnonymous,
    ShowNonAnonymous: ShowNonAnonymous
);