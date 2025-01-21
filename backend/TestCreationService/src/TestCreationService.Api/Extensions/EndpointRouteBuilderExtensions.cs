using ApiShared.interfaces;
using TestCreationService.Api.EndpointsFilters;

namespace TestCreationService.Api.Extensions;

internal static class EndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder TestEditPermissionRequired(this RouteHandlerBuilder builder) {
        return builder.AddEndpointFilter<TestEditPermissionFilter>();
    }
    public static RouteHandlerBuilder OnlyByTestCreator(this RouteHandlerBuilder builder) {
        return builder.AddEndpointFilter<UserIsTestCreatorFilter>();
    }
}
