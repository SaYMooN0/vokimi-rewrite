namespace TestManagingService.Domain.TestFeedbackRecordAggregate.general_test;

public record class GeneralTestFeedbackRecordsFilter(
    DateTime? CreatedDateFrom,
    DateTime? CreatedDateTo,
    ushort? TextLengthFrom,
    ushort? TextLengthTo,
    bool ShowAnonymous,
    bool ShowNonAnonymous,
    GeneralTestFeedbackRecordsSortOption SortOption
);