using ApiShared.extensions;
using TestManagingService.Api.Extensions;

namespace TestManagingService.Api.Endpoints;

internal static class ManageTestTagsHandlers
{
    internal static RouteGroupBuilder MapManageTestTagsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();
        
        //accept 
        //decline
        //ban
        //unban
        return group;
    }

}
