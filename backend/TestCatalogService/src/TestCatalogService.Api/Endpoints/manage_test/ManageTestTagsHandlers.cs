using ApiShared.extensions;
using TestCatalogService.Api.Extensions;

namespace TestCatalogService.Api.Endpoints.manage_test;

internal static class ManageTestTagsHandlers
{
    internal static RouteGroupBuilder MapManageTestTagsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();
        
        
        return group;
        return group;
    }

}