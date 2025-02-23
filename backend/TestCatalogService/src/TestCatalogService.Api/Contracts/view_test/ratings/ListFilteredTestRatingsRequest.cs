using ApiShared.interfaces;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.Common.filters;
using TestCatalogService.Domain.Common.sort_options;
using TestCatalogService.Domain.TestAggregate.formats_shared;

namespace TestCatalogService.Api.Contracts.view_test.ratings;

public record ListFilteredTestRatingsRequest() : IRequestWithValidationNeeded
{
    public int? MinRatingValue { get; init; } = null;
    public int? MaxRatingValue { get; init; } = null;
    public DateTime? CreationDateFrom { get; init; } = null;
    public DateTime? CreationDateTo { get; init; } = null;
    public DateTime? LastUpdateDateFrom { get; init; } = null;
    public DateTime? LastUpdateDateTo { get; init; } = null;
    public FilterTriState WereUpdated { get; init; } = FilterTriState.Unset;
    public FilterTriState ByUserFollowings { get; init; } = FilterTriState.Unset;
    public FilterTriState ByUserFollowers { get; init; } = FilterTriState.Unset;
    public TestRatingsSortOption Sorting { get; init; } = TestRatingsSortOption.Randomized;

    public RequestValidationResult Validate() {
        ErrList errs = new();
        if (MinRatingValue.HasValue && MinRatingValue < 0) {
            errs.Add(Err.ErrFactory.InvalidData("Minimum rating value cannot be less than 0"));
        }

        if (MaxRatingValue.HasValue && MaxRatingValue > TestRating.MaxValue) {
            errs.Add(Err.ErrFactory.InvalidData("Maximum rating value cannot be greater than allowed maximum"));
        }

        if (MinRatingValue.HasValue && MaxRatingValue.HasValue && MinRatingValue > MaxRatingValue) {
            errs.Add(Err.ErrFactory.InvalidData("Minimum rating value cannot be greater than maximum rating value"));
        }

        if (CreationDateFrom.HasValue && CreationDateTo.HasValue && CreationDateFrom > CreationDateTo) {
            errs.Add(Err.ErrFactory.InvalidData("Creation from date cannot be later than creation to date"));
        }

        if (LastUpdateDateFrom.HasValue && LastUpdateDateTo.HasValue && LastUpdateDateFrom > LastUpdateDateTo) {
            errs.Add(Err.ErrFactory.InvalidData("Last update from date cannot be later than last update to date"));
        }

        return errs;
    }

    public ErrOr<ListTestRatingsFilter> ParseToFilter(IDateTimeProvider dateTimeProvider, AppUserId? viewer) {
        if (viewer is null) {
            if (ByUserFollowings != FilterTriState.Unset) {
                return Err.ErrFactory.Unauthorized(
                    "Couldn't filter ratings by user followings because user is not logged in",
                    details: "Login in or set 'by user followings' filter field to unset"
                );
            }

            if (ByUserFollowers != FilterTriState.Unset) {
                return Err.ErrFactory.Unauthorized(
                    "Couldn't filter ratings by user followers because user is not logged in",
                    details: "Login in or set 'by user followers' filter field to unset"
                );
            }
        }

        var current = dateTimeProvider.Now.AddDays(1);
        if (CreationDateFrom.HasValue && CreationDateFrom > current) {
            return Err.ErrFactory.InvalidData("Creation Date From cannot be later than current date");
        }

        if (CreationDateTo.HasValue && CreationDateTo > current) {
            return Err.ErrFactory.InvalidData("Creation Date To cannot be later than current date");
        }

        if (LastUpdateDateFrom.HasValue && LastUpdateDateFrom > current) {
            return Err.ErrFactory.InvalidData("Last Update Date From cannot be later than current date");
        }

        if (LastUpdateDateTo.HasValue && LastUpdateDateTo > current) {
            return Err.ErrFactory.InvalidData("Last Update Date To cannot be later than current date");
        }

        return new ListTestRatingsFilter(
            (ushort?)MinRatingValue,
            (ushort?)MaxRatingValue,
            CreationDateFrom,
            CreationDateTo,
            LastUpdateDateFrom,
            LastUpdateDateTo,
            WereUpdated,
            ByUserFollowings,
            ByUserFollowers,
            Sorting
        );
    }
}