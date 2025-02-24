using TestCatalogService.Api.EndpointsFilters;

namespace TestCatalogService.Api.Extensions;

internal static class RouteGroupBuilderExtensions
{
    public static RouteGroupBuilder GroupUserAccessToViewTestRequired(this RouteGroupBuilder builder) {
        return builder.AddEndpointFilter<CheckUserAccessToViewTestEndpointFilter>();
    }
    public static RouteGroupBuilder GroupUserAccessToManageTestRequired(this RouteGroupBuilder builder) {
        return builder.AddEndpointFilter<CheckUserAccessToManageTestEndpointFilter>();
    }
}
