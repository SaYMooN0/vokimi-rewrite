using ApiShared.extensions;
using MediatR;
using TestCatalogService.Api.Contracts.view_test.comments;
using TestCatalogService.Api.Extensions;

namespace TestCatalogService.Api.Endpoints.view_test;

internal static class ViewTestCommentsHandlers
{
    internal static RouteGroupBuilder MapViewTestCommentsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToViewTestRequired();
        group.MapGet("/{package}", ListComments);
        group.MapPost("/filtered/{package}", ListCommentsFiltered)
            .WithRequestValidation<ListCommentsFilteredRequest>();

        //vote comment
        //add comment
        //add answer

        return group;
    }

    private static Task ListComments(
        HttpContext context,
        ISender mediator,
        int package
    ) {
    }

    private static Task ListCommentsFiltered(
        HttpContext context,
        ISender mediator,
        int package
    ) {
    }
}