using ApiShared.extensions;
using TestCatalogService.Api.Extensions;

namespace TestCatalogService.Api.Endpoints;

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