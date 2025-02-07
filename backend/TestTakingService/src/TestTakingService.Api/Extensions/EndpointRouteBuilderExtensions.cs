using TestTakingService.Api.EndpointsFilters;

namespace TestTakingService.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder WithUserAccessToTakeTestChecking(this RouteHandlerBuilder builder) {
        return builder.AddEndpointFilter<CheckUserAccessToTakeTestEndpointFilter>();
    }
}