using ApiShared.interfaces;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using SharedKernel.Common.tests.tier_list_format.feedback;
using TestManagingService.Domain.FeedbackRecordAggregate.filters;
using TestManagingService.Domain.FeedbackRecordAggregate.sort_options;

namespace TestManagingService.Api.Contracts.test_feedback.tier_list_test_format.feedback_records;

public class ListFilteredTierListTestFeedbackRecordsRequest : IRequestWithValidationNeeded
{
    public DateTime? CreationDateFrom { get; init; } = null;
    public DateTime? CreationDateTo { get; init; } = null;
    public int? MinTextLength { get; init; } = null;
    public int? MaxTextLength { get; init; } = null;
    public bool ShowAnonymous { get; init; } = true;
    public bool ShowNonAnonymous { get; init; } = true;

    public TierListTestFeedbackRecordsSortOption SortOption { get; init; } =
        TierListTestFeedbackRecordsSortOption.Randomized;

    public RequestValidationResult Validate() {
        ErrList errs = new();

        if (CreationDateFrom.HasValue && CreationDateTo.HasValue && CreationDateFrom > CreationDateTo) {
            errs.Add(Err.ErrFactory.InvalidData("Creation Date From cannot be later than Creation Date To"));
        }

        if (CreationDateFrom.HasValue && CreationDateFrom.Value.Year < 2020) {
            errs.Add(Err.ErrFactory.InvalidData("Creation Date From cannot be earlier than the year 2020"));
        }

        if (CreationDateTo.HasValue && CreationDateTo.Value.Year < 2020) {
            errs.Add(Err.ErrFactory.InvalidData("Creation Date To cannot be earlier than the year 2020"));
        }

        if (MinTextLength.HasValue && MaxTextLength.HasValue && MinTextLength > MaxTextLength) {
            errs.Add(Err.ErrFactory.InvalidData("Minimum text length cannot be greater than maximum text length"));
        }

        if (MaxTextLength.HasValue && MaxTextLength > TierListTestFeedbackRules.MaxPossibleFeedbackLength) {
            errs.Add(Err.ErrFactory.InvalidData(
                $"Maximum text length cannot exceed {TierListTestFeedbackRules.MaxPossibleFeedbackLength}"));
        }

        if (MinTextLength.HasValue && MinTextLength > TierListTestFeedbackRules.MaxPossibleFeedbackLength) {
            errs.Add(Err.ErrFactory.InvalidData(
                $"Minimum text length cannot exceed {TierListTestFeedbackRules.MaxPossibleFeedbackLength}"));
        }

        if (!ShowAnonymous && !ShowNonAnonymous) {
            errs.Add(Err.ErrFactory.InvalidData(
                "If both ShowAnonymous and ShowNonAnonymous are disabled, there will be no feedback"));
        }

        return errs;
    }

    public ErrOr<TierListTestFeedbackRecordsQueryFilter> GetParsedFilter(IDateTimeProvider dateTimeProvider) {
        if (CreationDateTo.HasValue && CreationDateTo > dateTimeProvider.Now) {
            return Err.ErrFactory.InvalidData("Creation Date To cannot be in the future.");
        }

        if (MinTextLength.HasValue && MinTextLength < 0) {
            return Err.ErrFactory.InvalidData("Minimum text length cannot be negative");
        }

        if (MaxTextLength.HasValue && MaxTextLength < 0) {
            return Err.ErrFactory.InvalidData("Maximum text length cannot be negative");
        }

        if (MaxTextLength.HasValue && MaxTextLength > ushort.MaxValue) {
            return Err.ErrFactory.InvalidData(
                $"Maximum text length value is set to a very big number. Please reduce it"
            );
        }

        return new TierListTestFeedbackRecordsQueryFilter(
            CreatedDateFrom: CreationDateFrom,
            CreatedDateTo: CreationDateTo,
            TextLengthFrom: MinTextLength.HasValue ? (ushort?)MinTextLength.Value : null,
            TextLengthTo: MaxTextLength.HasValue ? (ushort?)MaxTextLength.Value : null,
            ShowAnonymous: ShowAnonymous,
            ShowNonAnonymous: ShowNonAnonymous,
            SortOption
        );
    }
}