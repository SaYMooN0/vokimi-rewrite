using ApiShared.extensions;
using TestManagingService.Api.Extensions;

namespace TestManagingService.Api.Endpoints.statistics;

internal static class ManageTestSharedStatisticsHandlers
{
    internal static RouteGroupBuilder MapManageTestSharedStatisticsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();

        //statistics
        // group.MapGet("/", GetTestBaseStatistics);


        //test takings + filter
        //ratings + filter
        //comments + filter

        return group;
    }
}