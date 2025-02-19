namespace TestCatalogService.Domain.Common.filters;

public record class ListTestRatingsFilter(
    ushort? MinRatingValue,
    ushort? MaxRatingValue,
    DateTime? DateFrom,
    DateTime? DateTo,
    FilterTriState ByUserFollowings,
    FilterTriState ByUserFollowers
) { }