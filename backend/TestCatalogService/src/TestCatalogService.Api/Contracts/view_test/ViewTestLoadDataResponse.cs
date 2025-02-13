namespace TestCatalogService.Api.Contracts.view_test;

internal record class ViewTestLoadDataResponse(
    bool RatingsEnabled,
    bool CommentsViewEnabled,
    bool CommentsLeavingEnabled
)
{
    internal static ViewTestLoadDataResponse Create() => new();
}