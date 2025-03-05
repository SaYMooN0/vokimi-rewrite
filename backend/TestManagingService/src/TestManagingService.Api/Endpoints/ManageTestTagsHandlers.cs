using ApiShared.extensions;
using TestManagingService.Api.Extensions;

namespace TestManagingService.Api.Endpoints;

internal static class ManageTestTagsHandlers
{
    internal static RouteGroupBuilder MapManageTestTagsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();
        
        group.MapGet("/list", ...);
        group.MapPost("/update", ..).);
        
        group.MapGet("/listTagSuggestions", ...);
        group.MapPost("/acceptTagSuggestions", ...);
        group.MapPost("/declineTagSuggestions", ...);
        group.MapPost("/declineAndBanTagSuggestions", ...);
        return group;
    }

}
