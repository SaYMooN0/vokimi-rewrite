using ApiShared.extensions;
using TestManagingService.Api.Extensions;

namespace TestManagingService.Api.Endpoints;

internal static class ManageTestFeedbackHandlers
{
    internal static RouteGroupBuilder MapManageTestFeedbackHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();
        
        
        return group;
    }
}