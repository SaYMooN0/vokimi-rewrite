using TestCatalogService.Domain.TestAggregate;

namespace TestCatalogService.Api.Contracts.view_test;

internal record class ViewTestLoadDataResponse(
    bool RatingsEnabled,
    bool CommentsEnabled
)
{
    internal static ViewTestLoadDataResponse Create(BaseTest test) => new(
        test.InteractionsAccessSettings.AllowRatings.IsEnabled,
        test.InteractionsAccessSettings.AllowComments.IsEnabled
    );
}