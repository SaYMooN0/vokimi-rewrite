using TestManagingService.Domain.FeedbackRecordAggregate.sort_options;

namespace TestManagingService.Domain.FeedbackRecordAggregate.filters;

public record class GeneralTestFeedbackRecordsQueryFilter(
    DateTime? CreatedDateFrom,
    DateTime? CreatedDateTo,
    ushort? TextLengthFrom,
    ushort? TextLengthTo,
    bool ShowAnonymous,
    bool ShowNonAnonymous,
    GeneralTestFeedbackRecordsSortOption SortOption
) : BaseTestFeedbackRecordsQueryFilter
(
    CreatedDateFrom: CreatedDateFrom,
    CreatedDateTo: CreatedDateTo,
    ShowAnonymous: ShowAnonymous,
    ShowNonAnonymous: ShowNonAnonymous
);