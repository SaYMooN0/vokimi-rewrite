using TestCatalogService.Api.Extensions;

namespace TestCatalogService.Api.Endpoints.view_test;

internal static class ViewTestRatingsHandlers
{
    internal static RouteGroupBuilder MapViewTestRatingsHandlers(this RouteGroupBuilder group) {
        group
            .GroupUserAccessToViewTestRequired();

        return group;
    }
}