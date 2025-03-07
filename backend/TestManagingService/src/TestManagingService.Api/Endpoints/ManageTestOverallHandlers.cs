using ApiShared.extensions;
using TestManagingService.Api.Extensions;

namespace TestManagingService.Api.Endpoints;

internal static class ManageTestOverallHandlers
{
    internal static RouteGroupBuilder MapManageTestOverallHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();

        //get (TestFormat)

        //update interaction settings
        return group;
    }
}