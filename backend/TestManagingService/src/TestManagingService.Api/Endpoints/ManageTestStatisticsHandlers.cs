using ApiShared.extensions;
using TestManagingService.Api.Extensions;

namespace TestManagingService.Api.Endpoints;

internal static class ManageTestStatisticsHandlers
{
    internal static RouteGroupBuilder MapManageTestStatisticsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();

        //statistics
        
        //test takings + filter
        //ratings + filter
        //comments + filter

        return group;
    }
}