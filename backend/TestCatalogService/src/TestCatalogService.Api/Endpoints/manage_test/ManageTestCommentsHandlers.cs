using ApiShared.extensions;
using TestCatalogService.Api.Extensions;

namespace TestCatalogService.Api.Endpoints.manage_test;

internal static class ManageTestCommentsHandlers
{
    internal static RouteGroupBuilder MapManageTestCommentsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();
        

        //search?

        //hide unhide
        //reported comments
        //mark as spoiler
        return group;
    }
}