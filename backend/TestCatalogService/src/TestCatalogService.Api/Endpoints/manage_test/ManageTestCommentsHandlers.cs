using ApiShared.extensions;
using TestCatalogService.Api.Extensions;

namespace TestCatalogService.Api.Endpoints.manage_test;

internal static class ManageTestCommentsHandlers
{
    internal static RouteGroupBuilder MapManageTestCommentsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();
        
        group.MapGet("/list/{package}", ListComments);
        group.MapPost("/listFiltered/{package}", ListCommentsFiltered)
            .WithRequestValidation<ListCommentsFilteredRequest>();
        //hide unhide
        //reported comments
        //mark as spoiler
        return group;
    }
}