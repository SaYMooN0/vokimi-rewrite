using TestManagingService.Api.EndpointsFilters;

namespace TestManagingService.Api.Extensions;

internal static class RouteGroupBuilderExtensions
{
    public static RouteGroupBuilder GroupUserAccessToManageTestRequired(this RouteGroupBuilder builder) {
        return builder.AddEndpointFilter<CheckUserAccessToManageTestEndpointFilter>();
    }

    public static RouteGroupBuilder GroupUserAccessToViewTestRequired(this RouteGroupBuilder builder) {
        return builder.AddEndpointFilter<CheckUserAccessToViewTestEndpointFilter>();
    }
}