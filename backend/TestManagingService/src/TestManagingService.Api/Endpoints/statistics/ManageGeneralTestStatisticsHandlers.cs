using ApiShared.extensions;
using TestManagingService.Api.Extensions;

namespace TestManagingService.Api.Endpoints.statistics;

internal static class ManageGeneralTestStatisticsHandlers
{
    internal static RouteGroupBuilder MapManageGeneralTestStatisticsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();

        //statistics
        
        //time on each question

        return group;
    }
}