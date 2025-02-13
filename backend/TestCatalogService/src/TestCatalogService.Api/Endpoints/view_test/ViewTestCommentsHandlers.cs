using TestCatalogService.Api.Extensions;

namespace TestCatalogService.Api.Endpoints.view_test;

internal static class ViewTestCommentsHandlers
{
    internal static RouteGroupBuilder MapViewTestCommentsHandlers(this RouteGroupBuilder group) {
        group
            .GroupUserAccessToViewTestRequired();

        return group;
    }
}