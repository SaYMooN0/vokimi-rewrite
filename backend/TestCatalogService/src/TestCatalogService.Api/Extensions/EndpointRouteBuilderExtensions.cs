using TestCatalogService.Api.EndpointsFilters;

namespace TestCatalogService.Api.Extensions;

internal static class EndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder WithAccessCheckToRateTest(this RouteHandlerBuilder builder) {
        return builder.AddEndpointFilter<CheckUserAccessRateTestEndpointFilter>();
    }
    public static RouteHandlerBuilder WithAccessCheckToCommentTest(this RouteHandlerBuilder builder) {
        return builder.AddEndpointFilter<CheckUserAccessToCommentTestEndpointFilter>();
    } 
    public static RouteHandlerBuilder WithCommentBelongsToTestCheck(this RouteHandlerBuilder builder) {
        return builder.AddEndpointFilter<CheckCommentBelongsToTestEndpointFilter>();
    }
}
