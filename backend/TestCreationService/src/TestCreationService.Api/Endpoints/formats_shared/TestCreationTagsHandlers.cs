using ApiShared;
using ApiShared.extensions;
using MediatR;
using TestCreationService.Api.Contracts.Tests.test_creation.formats_shared;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.Tests.formats_shared.commands.tags;

namespace TestCreationService.Api.Endpoints.formats_shared;

internal static class TestCreationTagsHandlers
{
    internal static RouteGroupBuilder MapTestCreationTagsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired();

        group.MapGet("/list", ListTestTags);
        group.MapPost("/update", UpdateTestTags)
            .WithRequestValidation<UpdateTestTagsRequest>();
        group.MapPost("/clear", ClearTestTags);
        return group;
    }
    private async static Task<IResult> ListTestTags(
       HttpContext httpContext,
       ISender mediator
    ) {
        var testId = httpContext.GetTestIdFromRoute();

        ListTestTagsCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            (tags) => Results.Json(new { Tags = tags })
        );
    }
    private async static Task<IResult> UpdateTestTags(
       HttpContext httpContext,
       ISender mediator
    ) {
        var testId = httpContext.GetTestIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateTestTagsRequest>();

        UpdateTestTagsCommand command = new(testId, request.Tags);
        var result = await mediator.Send(command);

        return CustomResults.FromErrListOr(
            result,
            (tags) => Results.Json(new { NewTags = tags })
        );
    }
    private async static Task<IResult> ClearTestTags(
       HttpContext httpContext,
       ISender mediator
    ) {
        var testId = httpContext.GetTestIdFromRoute();

        ClearTestTagsCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
        );
    }
}
