using TestCatalogService.Api.EndpointsFilters;

namespace TestCatalogService.Api.Extensions;

internal static class EndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder WithAccessCheckToCommentTest(this RouteHandlerBuilder builder) {
        return builder.AddEndpointFilter<CheckUserAccessToCommentTestEndpointFilter>();
    } 
}
