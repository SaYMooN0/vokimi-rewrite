using System.Collections.Immutable;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.Common.sort_options;

namespace TestCatalogService.Domain.TestAggregate.formats_shared.ratings;

internal static class TestRatingsQueryExtensions
{
    internal static IEnumerable<TestRating> ApplySorting(
        this IEnumerable<TestRating> ratings,
        TestRatingsSortOption sorting
    ) => sorting switch {
        TestRatingsSortOption.RecentlyUpdated => ratings.OrderByDescending(r => r.LastUpdated ?? r.CreatedAt),
        TestRatingsSortOption.RecentlyAdded => ratings.OrderByDescending(r => r.CreatedAt),
        TestRatingsSortOption.HighestValue => ratings.OrderByDescending(r => r.Value),
        TestRatingsSortOption.LowestValue => ratings.OrderBy(r => r.Value),
        TestRatingsSortOption.Randomized => ratings.OrderBy(_ => Guid.NewGuid()),
        _ => ratings
    };

    internal static IEnumerable<TestRating> WithRatingValueFilter(
        this IEnumerable<TestRating> ratings,
        ushort? minRatingValue,
        ushort? maxRatingValue
    ) => (minRatingValue, maxRatingValue) switch {
        (null, null) => ratings,
        (null, _) => ratings.Where(r => r.Value <= maxRatingValue),
        (_, null) => ratings.Where(r => r.Value >= minRatingValue),
        _ => ratings.Where(r => r.Value >= minRatingValue && r.Value <= maxRatingValue)
    };

    internal static IEnumerable<TestRating> WithCreationDateFilter(
        this IEnumerable<TestRating> ratings,
        DateTime? dateFrom,
        DateTime? dateTo
    ) => (dateFrom, dateTo) switch {
        (null, null) => ratings,
        (null, _) => ratings.Where(r => r.CreatedAt <= dateTo),
        (_, null) => ratings.Where(r => r.CreatedAt >= dateFrom),
        _ => ratings.Where(r => r.CreatedAt >= dateTo && r.CreatedAt <= dateFrom)
    };

    internal static IEnumerable<TestRating> WithUpdatingDateFilter(
        this IEnumerable<TestRating> ratings,
        DateTime? dateFrom,
        DateTime? dateTo
    ) => (dateFrom, dateTo) switch {
        (null, null) => ratings,
        (null, _) => ratings.Where(r => (r.LastUpdated ?? r.CreatedAt) <= dateTo),
        (_, null) => ratings.Where(r => (r.LastUpdated ?? r.CreatedAt) >= dateFrom),
        _ => ratings.Where(r => (r.LastUpdated ?? r.CreatedAt) >= dateFrom && (r.LastUpdated ?? r.CreatedAt) <= dateTo)
    };


    internal static IEnumerable<TestRating> WithWereUpdatedFilter(
        this IEnumerable<TestRating> ratings,
        FilterTriState wereUpdated
    ) => wereUpdated switch {
        FilterTriState.Yes => ratings.Where(r => r.LastUpdated.HasValue),
        FilterTriState.No => ratings.Where(r => !r.LastUpdated.HasValue),
        _ => ratings
    };

    internal static async Task<IEnumerable<TestRating>> WithByFollowingsFilter(
        IEnumerable<TestRating> ratings,
        FilterTriState byUserFollowing,
        Func<Task<ImmutableHashSet<AppUserId>>> getUserFollowings
    ) {
        if (byUserFollowing == FilterTriState.Unset) {
            return ratings;
        }

        var userFollowings = await getUserFollowings();

        return byUserFollowing switch {
            FilterTriState.Yes => ratings.Where(r => userFollowings.Contains(r.UserId)),
            FilterTriState.No => ratings.Where(r => !userFollowings.Contains(r.UserId)),
            _ => ratings
        };
    }

    internal static async Task<IEnumerable<TestRating>> WithByFollowersFilter(
        IEnumerable<TestRating> ratings,
        FilterTriState byUserFollowers,
        Func<Task<ImmutableHashSet<AppUserId>>> getUserFollowers
    ) {
        if (byUserFollowers == FilterTriState.Unset) {
            return ratings;
        }

        var userFollowers = await getUserFollowers();

        return byUserFollowers switch {
            FilterTriState.Yes => ratings.Where(r => userFollowers.Contains(r.UserId)),
            FilterTriState.No => ratings.Where(r => !userFollowers.Contains(r.UserId)),
            _ => ratings
        };
    }
}