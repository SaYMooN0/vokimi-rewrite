using ApiShared.extensions;
using TestCatalogService.Api.Extensions;

namespace TestCatalogService.Api.Endpoints.manage_test;

internal static class ManageTestStatisticsHandlers
{
    internal static RouteGroupBuilder MapManageTestStatisticsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();
        
        
        return group;
    }
}