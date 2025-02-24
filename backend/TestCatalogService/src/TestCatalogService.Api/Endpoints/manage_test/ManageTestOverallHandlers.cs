using ApiShared.extensions;
using TestCatalogService.Api.Extensions;

namespace TestCatalogService.Api.Endpoints.manage_test;

internal static class ManageTestOverallHandlers
{
    internal static RouteGroupBuilder MapManageTestOverallHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();
        return group;
    }
}