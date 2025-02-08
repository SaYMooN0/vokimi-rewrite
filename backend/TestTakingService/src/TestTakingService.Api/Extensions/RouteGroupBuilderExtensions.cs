
using TestTakingService.Api.EndpointsFilters;

namespace TestTakingService.Api.Extensions;

internal static class RouteGroupBuilderExtensions
{
    public static RouteGroupBuilder GroupUserAccessToTakeTestRequired(this RouteGroupBuilder builder) {
        return builder.AddEndpointFilter<CheckUserAccessToTakeTestEndpointFilter>();
    }
}
