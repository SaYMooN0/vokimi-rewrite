using System.Collections.Immutable;
using TestCatalogService.Domain.TestAggregate.formats_shared;
using TestCatalogService.Domain.TestAggregate.formats_shared.ratings;

namespace TestCatalogService.Api.Contracts.view_test.ratings;

internal record class ViewTestRatingsListResponse(
    TestRatingDataViewResponse[] Ratings
)
{
    public static ViewTestRatingsListResponse FromTestRatings(ImmutableArray<TestRating> testRatings) =>
        new(testRatings.Select(TestRatingDataViewResponse.FromTestRating).ToArray());
}

internal record class TestRatingDataViewResponse(
    string AppUserId,
    ushort Value,
    DateTime CreatedAt,
    DateTime? UpdatedAt
)
{
    internal static TestRatingDataViewResponse FromTestRating(TestRating rating) => new(
        rating.UserId.ToString(),
        rating.Value,
        rating.CreatedAt,
        rating.LastUpdated
    );
}