using TestCatalogService.Domain.Common.sort_options;

namespace TestCatalogService.Domain.Common.filters;

public record class ListTestRatingsFilter(
    ushort? MinRatingValue,
    ushort? MaxRatingValue,
    DateTime? CreationDateFrom,
    DateTime? CreationDateTo,
    DateTime? LastUpdateDateFrom,
    DateTime? LastUpdateDateTo,
    FilterTriState WereUpdated,
    //      auth requiring
    FilterTriState ByUserFollowings,
    FilterTriState ByUserFollowers,
    //      sort
    TestRatingsSortOption Sorting
) { }