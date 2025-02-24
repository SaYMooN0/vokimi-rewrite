using ApiShared.extensions;
using TestCatalogService.Api.Extensions;

namespace TestCatalogService.Api.Endpoints.manage_test;

internal static class ManageTestFeedbackHandlers
{
    internal static RouteGroupBuilder MapManageTestFeedbackHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();
        
        
        return group;
    }
}