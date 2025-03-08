namespace TestManagingService.Api.Contracts.statistics.formats_shared;

public record class BaseTestStatisticsResponse(
    uint TotalTestTakingsCount,
    uint TotalRatingsCount,
    double AverageRating,
    uint TotalCommentsCount
)
{
    
}