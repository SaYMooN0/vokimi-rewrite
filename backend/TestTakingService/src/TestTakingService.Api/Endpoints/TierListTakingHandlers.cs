using TestTakingService.Api.Extensions;

namespace TestTakingService.Api.Endpoints;

internal static class TierListTakingHandlers
{
    internal static RouteGroupBuilder MapTierListTakingHandlers(this RouteGroupBuilder group) {
        group
            .GroupUserAccessToTakeTestRequired();

        // group.MapGet("/loadTestTakingData", );
        // group.MapPost("/testTaken", )
        //     .WithRequestValidation<>();
        
        return group;
    }
}