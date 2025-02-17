using ApiShared.interfaces;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestCatalogService.Application.Common;
using TestCatalogService.Application.Common.filters;
using TestCatalogService.Application.Common.sort_options;

namespace TestCatalogService.Api.Contracts.view_test.comments;

public class ListCommentsFilteredRequest : IRequestWithValidationNeeded
{
    public int? MinAnswersCount { get; init; }
    public int? MaxAnswersCount { get; init; }
    public int? MinVotesRating { get; init; }
    public int? MaxVotesRating { get; init; }
    public int? MinVotesCount { get; init; }
    public int? MaxVotesCount { get; init; }
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
    public bool ShowHidden { get; init; }
    public bool ShowDeleted { get; init; }
    public FilterTriState ByUser { get; init; }
    public FilterTriState WithAttachments { get; init; }
    public FilterTriState ByUserFollowings { get; init; }
    public FilterTriState ByUserFollowers { get; init; }
    public TestCommentsSortOption Sorting { get; init; }

    public RequestValidationResult Validate() {
        ErrList errs = new();
        //answers
        if (MinAnswersCount.HasValue && MinAnswersCount < 0) {
            errs.Add(Err.ErrFactory.InvalidData("Minimum answers count cannot be less than 0"));
        }

        if (MaxAnswersCount.HasValue && MaxAnswersCount < 0) {
            errs.Add(Err.ErrFactory.InvalidData("Maximum answers count cannot be less than 0"));
        }

        if (MinAnswersCount.HasValue && MaxAnswersCount.HasValue && MinAnswersCount > MaxAnswersCount) {
            errs.Add(Err.ErrFactory.InvalidData("Minimum answers count cannot be greater than maximum answers count"));
        }

        //votes rating 
        if (MinVotesRating.HasValue && MaxVotesRating.HasValue && MinVotesRating > MaxVotesRating) {
            errs.Add(Err.ErrFactory.InvalidData("Minimum votes rating cannot be greater than maximum votes rating"));
        }

        //votes count
        if (MinVotesCount.HasValue && MinVotesCount < 0) {
            errs.Add(Err.ErrFactory.InvalidData("Minimum votes count cannot be less than 0"));
        }

        if (MaxVotesCount.HasValue && MaxVotesCount < 0) {
            errs.Add(Err.ErrFactory.InvalidData("Maximum votes count cannot be less than 0"));
        }

        if (MinVotesCount.HasValue && MaxVotesCount.HasValue && MinVotesCount > MaxVotesCount) {
            errs.Add(Err.ErrFactory.InvalidData("Minimum votes count cannot be greater than maximum votes count"));
        }

        //date
        if (DateFrom.HasValue && DateTo.HasValue && DateFrom > DateTo) {
            errs.Add(Err.ErrFactory.InvalidData("Start date cannot be later than end date"));
        }

        return errs;
    }

    public ErrOr<ListTestCommentsFilter> ParseToFilter(IDateTimeProvider dateTimeProvider, AppUserId viewer) {
        if (DateFrom.HasValue && DateFrom > dateTimeProvider.Now.AddDays(1)) {
            return Err.ErrFactory.InvalidData("Date From cannot be later than current date");
        }

        if (DateTo.HasValue && DateTo > dateTimeProvider.Now.AddDays(1)) {
            return Err.ErrFactory.InvalidData("Date To cannot be later than current date");
        }

        if (ByUser != FilterTriState.Unset && viewer is null) {
            return Err.ErrFactory.InvalidData("Authentication is required to filter by User's comments");
        }

        if (ByUserFollowings != FilterTriState.Unset && viewer is null) {
            return Err.ErrFactory.InvalidData("Authentication is required to filter by User Followings' comments");
        }

        if (ByUserFollowers != FilterTriState.Unset && viewer is null) {
            return Err.ErrFactory.InvalidData("Authentication is required to filter by User Followers' comments");
        }


        return new ListTestCommentsFilter(
            (uint?)MinAnswersCount,
            (uint?)MaxAnswersCount,
            MinVotesRating,
            MaxVotesRating,
            (uint?)MinVotesCount,
            (uint?)MaxVotesCount,
            DateFrom: DateFrom,
            DateTo: DateTo,
            ShowHidden: ShowHidden,
            ShowDeleted: ShowDeleted,
            WithAttachments,
            ByUser: ByUser,
            ByUserFollowings: ByUserFollowings,
            ByUserFollowers: ByUserFollowers,
            Sorting
        );
    }
}